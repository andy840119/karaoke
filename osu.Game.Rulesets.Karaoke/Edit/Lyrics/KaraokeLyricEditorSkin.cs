﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    /// <summary>
    /// This karaoke skin is using in lyric editor only.
    /// </summary>
    public class KaraokeLyricEditorSkin : KaraokeSkin
    {
        public const int MIN_FONT_SIZE = 10;
        public const int MAX_FONT_SIZE = 45;

        internal static readonly Guid DEFAULT_SKIN = new("FEC5A290-5709-11EC-9F10-0800200C9A66");

        public static SkinInfo CreateInfo() => new()
        {
            ID = DEFAULT_SKIN,
            Name = "karaoke! (default editor skin)",
            Creator = "team karaoke!",
            Protected = true,
            InstantiationInfo = typeof(DefaultKaraokeSkin).GetInvariantInstantiationInfo(),
        };

        public KaraokeLyricEditorSkin(IStorageResourceProvider resources)
            : this(CreateInfo(), resources)
        {
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        public KaraokeLyricEditorSkin(SkinInfo skin, IStorageResourceProvider resources)
            : base(skin, resources)
        {
            // TODO : need a better way to load resource
            var assembly = Assembly.GetExecutingAssembly();
            const string resource_name = @"osu.Game.Rulesets.Karaoke.Resources.Skin.editor.skin";

            using (var stream = assembly.GetManifestResourceStream(resource_name))
            using (var reader = new LineBufferedReader(stream))
            {
                var karaokeSkin = new KaraokeSkinDecoder().Decode(reader);

                // Default values
                BindableDefaultLyricConfig.Value = karaokeSkin.DefaultLyricConfig;
                BindableDefaultLyricStyle.Value = karaokeSkin.DefaultLyricStyle;
                BindableDefaultNoteStyle.Value = karaokeSkin.DefaultNoteStyle;

                // Create bindable
                for (int i = 0; i < karaokeSkin.Layouts.Count; i++)
                    BindableLayouts.Add(i, new Bindable<LyricLayout>(karaokeSkin.Layouts[i]));
                for (int i = 0; i < karaokeSkin.LyricStyles.Count; i++)
                    BindableLyricStyles.Add(i, new Bindable<LyricStyle>(karaokeSkin.LyricStyles[i]));
                for (int i = 0; i < karaokeSkin.NoteStyles.Count; i++)
                    BindableNoteStyles.Add(i, new Bindable<NoteStyle>(karaokeSkin.NoteStyles[i]));

                // Create lookups
                BindableFontsLookup.Value = karaokeSkin.LyricStyles.ToDictionary(k => karaokeSkin.LyricStyles.IndexOf(k), y => y.Name);
                BindableLayoutsLookup.Value = karaokeSkin.Layouts.ToDictionary(k => karaokeSkin.Layouts.IndexOf(k), y => y.Name);
                BindableNotesLookup.Value = karaokeSkin.NoteStyles.ToDictionary(k => karaokeSkin.NoteStyles.IndexOf(k), y => y.Name);
            }

            FontSize = 26;
        }

        protected Bindable<LyricConfig> BindableFont => BindableDefaultLyricConfig;

        public float FontSize
        {
            get => BindableFont.Value.MainTextFont.Size;
            set
            {
                float textSize = Math.Max(Math.Min(value, MAX_FONT_SIZE), MIN_FONT_SIZE);
                float changePercentage = textSize / FontSize;

                BindableFont.Value.MainTextFont
                    = multipleSize(BindableFont.Value.MainTextFont, changePercentage);
                BindableFont.Value.RubyTextFont
                    = multipleSize(BindableFont.Value.RubyTextFont, changePercentage);
                BindableFont.Value.RomajiTextFont
                    = multipleSize(BindableFont.Value.RomajiTextFont, changePercentage);

                BindableFont.TriggerChange();

                static FontUsage multipleSize(FontUsage origin, float percentage)
                    => origin.With(size: origin.Size * percentage);
            }
        }
    }
}
