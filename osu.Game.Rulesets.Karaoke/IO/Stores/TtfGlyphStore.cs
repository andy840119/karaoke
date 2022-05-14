﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Text;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using TextBuilder = SixLabors.ImageSharp.Drawing.TextBuilder;

namespace osu.Game.Rulesets.Karaoke.IO.Stores
{
    public class TtfGlyphStore : IResourceStore<TextureUpload>, IGlyphStore
    {
        public string FontName { get; }

        public float? Baseline => fontInstance?.LineHeight;

        [CanBeNull]
        public Font Font => completionSource.Task.Result;

        protected readonly string AssetName;

        protected readonly ResourceStore<byte[]> Store;

        protected int LoadedGlyphCount;
        private const int dpi = 80;

        private IFontInstance fontInstance => Font?.Instance;

        private readonly TaskCompletionSource<Font> completionSource = new();

        private Task fontLoadTask;

        /// <summary>
        ///     Create a new glyph store.
        /// </summary>
        /// <param name="store">The store to provide font resources.</param>
        /// <param name="assetName">The base name of thße font.</param>
        public TtfGlyphStore(ResourceStore<byte[]> store, string assetName = null)
        {
            Store = new ResourceStore<byte[]>(store);

            Store.AddExtension("ttf");

            AssetName = assetName;

            FontName = assetName?.Split('/').Last();
        }

        public Task LoadFontAsync()
        {
            return fontLoadTask ??= Task.Factory.StartNew(() =>
            {
                try
                {
                    Font font;

                    using (var s = Store.GetStream($@"{AssetName}"))
                    {
                        var fonts = new FontCollection();
                        var fontFamily = fonts.Install(s);
                        font = new Font(fontFamily, 1);
                    }

                    completionSource.SetResult(font);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, $"Couldn't load font asset from {AssetName}.");
                    completionSource.SetResult(null);
                    throw;
                }
            }, TaskCreationOptions.PreferFairness);
        }

        public bool HasGlyph(char c)
        {
            var glyph = fontInstance?.GetGlyph(c);
            return glyph?.GlyphType != GlyphType.Fallback;
        }

        [CanBeNull]
        public CharacterGlyph Get(char character)
        {
            if (fontInstance == null)
                return null;

            Debug.Assert(Baseline != null);

            var glyphInstance = fontInstance.GetGlyph(character);
            if (glyphInstance.GlyphType == GlyphType.Fallback)
                return null;

            string text = new(new[] { character });
            var style = new RendererOptions(Font, dpi);
            var bounds = TextMeasurer.MeasureBounds(text, style);

            float xOffset = bounds.Left * dpi;
            float yOffset = bounds.Top * dpi;

            int advanceWidth2 = glyphInstance.AdvanceWidth * dpi / glyphInstance.SizeOfEm;
            return new CharacterGlyph(character, xOffset, yOffset, advanceWidth2, Baseline.Value, this);
        }

        public int GetKerning(char left, char right)
        {
            if (fontInstance == null)
                return 0;

            var leftGlyphInstance = fontInstance.GetGlyph(left);
            var rightGlyphInstance = fontInstance.GetGlyph(right);

            // todo : got no idea why all offset is zero.
            float kerning = fontInstance.GetOffset(rightGlyphInstance, leftGlyphInstance).X;
            return (int)kerning;
        }

        public TextureUpload Get(string name)
        {
            if (fontInstance == null) return null;

            if (name.Length > 1 && !name.StartsWith($@"{FontName}/", StringComparison.Ordinal))
                return null;

            return !HasGlyph(name.Last()) ? null : LoadCharacter(name.Last());
        }

        public virtual async Task<TextureUpload> GetAsync(string name, CancellationToken cancellationToken = default)
        {
            if (name.Length > 1 && !name.StartsWith($@"{FontName}/", StringComparison.Ordinal))
                return null;

            await completionSource.Task.ConfigureAwait(false);

            return LoadCharacter(name.Last());
        }

        public Stream GetStream(string name)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<string> GetAvailableResources()
        {
            throw new NotSupportedException();
        }

        protected virtual TextureUpload LoadCharacter(char c)
        {
            if (Font == null)
                return null;

            LoadedGlyphCount++;

            // see: https://stackoverflow.com/a/53023454/4105113
            const float texture_scale = dpi;
            var style = new RendererOptions(Font, dpi);
            string text = new(new[] { c });
            var bounds = TextMeasurer.MeasureBounds(text, style);
            var targetSize = new
            {
                Width = (int)(bounds.Width * texture_scale),
                Height = (int)(bounds.Height * texture_scale)
            };

            // this is the important line, where we render the glyphs to a vector instead of directly to the image
            // this allows further vector manipulation (scaling, translating) etc without the expensive pixel operations.
            var glyphs = TextBuilder.GenerateGlyphs(text, style);

            // should calculate this because it will cut the border if width and height scale is not the same.
            float widthScale = targetSize.Width / glyphs.Bounds.Width;
            float heightScale = targetSize.Height / glyphs.Bounds.Height;
            float minScale = Math.Min(widthScale, heightScale);

            // scale so that it will fit exactly in image shape once rendered
            glyphs = glyphs.Scale(minScale);

            // move the vectorised glyph so that it touch top and left edges
            // could be tweeked to center horizontally & vertically here
            glyphs = glyphs.Translate(-glyphs.Bounds.Location);

            // create image with char.
            var img = new Image<Rgba32>(targetSize.Width, targetSize.Height, new Rgba32(255, 255, 255, 0));
            img.Mutate(i => i.Fill(Color.White, glyphs));
            return new TextureUpload(img);
        }

        Task<CharacterGlyph> IResourceStore<CharacterGlyph>.GetAsync(string name, CancellationToken cancellationToken)
        {
            return Task.Run(() => ((IGlyphStore)this).Get(name[0]), cancellationToken);
        }

        CharacterGlyph IResourceStore<CharacterGlyph>.Get(string name)
        {
            return Get(name[0]);
        }

        #region IDisposable Support

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        #endregion
    }
}
