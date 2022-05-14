﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Layout;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Skinning;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Skinning.Legacy
{
    public class LegacyNotePiece : LegacyKaraokeColumnElement
    {
        protected readonly Bindable<Color4> AccentColour = new();
        protected readonly Bindable<Color4> HitColour = new();

        private readonly IBindable<ScrollingDirection> direction = new Bindable<ScrollingDirection>();
        private readonly LayoutValue subtractionCache = new(Invalidation.DrawSize);
        private readonly IBindable<bool> isHitting = new Bindable<bool>();
        private readonly IBindable<bool> display = new Bindable<bool>();
        private readonly IBindableList<int> singer = new BindableList<int>();

        private LayerContainer background;
        private LayerContainer foreground;
        private LayerContainer border;

        public LegacyNotePiece()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;

            AddLayout(subtractionCache);
        }

        protected override void Update()
        {
            base.Update();

            if (!subtractionCache.IsValid && DrawWidth > 0)
            {
                // TODO : maybe do something
                subtractionCache.Validate();
            }
        }

        protected virtual void OnDirectionChanged(ValueChangedEvent<ScrollingDirection> direction)
        {
            if (direction.NewValue == ScrollingDirection.Left)
                InternalChildren.ForEach(x => x.Scale = Vector2.One);
            else
                InternalChildren.ForEach(x => x.Scale = new Vector2(-1, 1));
        }

        private static Sprite getSpriteFromLookup(ISkin skin, LegacyKaraokeSkinConfigurationLookups lookup, LegacyKaraokeSkinNoteLayer layer)
        {
            string name = getTextureNameFromLookup(lookup, layer);

            switch (layer)
            {
                case LegacyKaraokeSkinNoteLayer.Background:
                case LegacyKaraokeSkinNoteLayer.Border:
                    return getSpriteByName(name) ?? new Sprite();

                case LegacyKaraokeSkinNoteLayer.Foreground:
                    return getSpriteByName(name)
                           ?? getSpriteByName(getTextureNameFromLookup(lookup, LegacyKaraokeSkinNoteLayer.Background))
                           ?? new Sprite();

                default:
                    return null;
            }

            Sprite getSpriteByName(string spriteName) => (Sprite)skin.GetAnimation(spriteName, true, true).With(d =>
            {
                switch (d)
                {
                    case null:
                        return;

                    case TextureAnimation animation:
                        animation.IsPlaying = false;
                        break;
                }
            });
        }

        private static string getTextureNameFromLookup(LegacyKaraokeSkinConfigurationLookups lookup, LegacyKaraokeSkinNoteLayer layer)
        {
            string suffix = lookup switch
            {
                LegacyKaraokeSkinConfigurationLookups.NoteBodyImage => "body",
                LegacyKaraokeSkinConfigurationLookups.NoteHeadImage => "head",
                LegacyKaraokeSkinConfigurationLookups.NoteTailImage => "tail",
                _ => throw new ArgumentOutOfRangeException(nameof(lookup))
            };

            string layerSuffix = layer switch
            {
                LegacyKaraokeSkinNoteLayer.Border => "border",
                LegacyKaraokeSkinNoteLayer.Background => "background",
                _ => string.Empty
            };

            return $"karaoke-note-{layerSuffix}-{suffix}";
        }

        [BackgroundDependencyLoader]
        private void load(DrawableHitObject drawableObject, ISkinSource skin, IScrollingInfo scrollingInfo)
        {
            InternalChildren = new[]
            {
                background = createLayer("Background layer", skin, LegacyKaraokeSkinNoteLayer.Background),
                foreground = createLayer("Foreground layer", skin, LegacyKaraokeSkinNoteLayer.Foreground),
                border = createLayer("Border layer", skin, LegacyKaraokeSkinNoteLayer.Border)
            };

            var note = (DrawableNote)drawableObject;

            direction.BindTo(scrollingInfo.Direction);
            direction.BindValueChanged(OnDirectionChanged, true);
            isHitting.BindTo(note.IsHitting);
            display.BindTo(note.DisplayBindable);
            singer.BindTo(note.SingersBindable);

            AccentColour.BindValueChanged(onAccentChanged);
            HitColour.BindValueChanged(onAccentChanged);
            isHitting.BindValueChanged(onIsHittingChanged, true);
            display.BindValueChanged(_ => onAccentChanged(), true);
            singer.BindCollectionChanged((_, _) => applySingerStyle(skin, note.HitObject), true);
        }

        private void onIsHittingChanged(ValueChangedEvent<bool> isHitting)
        {
            // Update animate
            InternalChildren.OfType<LayerContainer>().ForEach(x =>
            {
                x.Reset();
                x.IsPlaying = isHitting.NewValue;
            });

            // Foreground sparkle
            foreground.ClearTransforms(false, nameof(foreground.Colour));
            foreground.Alpha = 0;

            if (!isHitting.NewValue)
                return;

            foreground.Alpha = 1;

            const float animation_length = 50;

            // wait for the next sync point
            double synchronisedOffset = animation_length * 2 - Time.Current % (animation_length * 2);
            using (foreground.BeginDelayedSequence(synchronisedOffset))
                foreground.FadeColour(AccentColour.Value.Lighten(0.7f), animation_length).Then().FadeColour(foreground.Colour, animation_length).Loop();
        }

        private void applySingerStyle(ISkinSource skin, Note note)
        {
            var noteSkin = skin?.GetConfig<Note, NoteStyle>(note)?.Value;
            if (noteSkin == null)
                return;

            AccentColour.Value = noteSkin.NoteColor;
            HitColour.Value = noteSkin.BlinkColor;
        }

        private LayerContainer createLayer(string name, ISkin skin, LegacyKaraokeSkinNoteLayer layer)
        {
            return new()
            {
                RelativeSizeAxes = Axes.Both,
                Name = name,
                Children = new[]
                {
                    getSpriteFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteHeadImage, layer).With(d =>
                    {
                        if (d == null)
                            return;

                        d.Name = "Head";
                        d.Anchor = Anchor.CentreLeft;
                        d.Origin = Anchor.Centre;
                    }),
                    getSpriteFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteBodyImage, layer).With(d =>
                    {
                        if (d == null)
                            return;

                        d.Name = "Body";
                        d.Anchor = Anchor.Centre;
                        d.Origin = Anchor.Centre;
                        d.Size = Vector2.One;
                        d.FillMode = FillMode.Stretch;
                        d.RelativeSizeAxes = Axes.X;
                        d.Depth = 1;

                        d.Height = d.Texture?.DisplayHeight ?? 0;
                    }),
                    getSpriteFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteTailImage, layer).With(d =>
                    {
                        if (d == null)
                            return;

                        d.Name = "Tail";
                        d.Anchor = Anchor.CentreRight;
                        d.Origin = Anchor.Centre;
                    })
                }
            };
        }

        private void onAccentChanged()
        {
            onAccentChanged(new ValueChangedEvent<Color4>(AccentColour.Value, AccentColour.Value));
        }

        private void onAccentChanged(ValueChangedEvent<Color4> accent)
        {
            foreground.Colour = HitColour.Value;
            background.Colour = display.Value ? accent.NewValue : new Color4(23, 41, 46, 255);

            subtractionCache.Invalidate();
        }

        private class LayerContainer : Container
        {
            public IEnumerable<TextureAnimation> AnimateChildren => Children.OfType<TextureAnimation>();

            public bool IsPlaying
            {
                set => AnimateChildren.ForEach(d => d.IsPlaying = value);
            }

            public void Reset()
            {
                AnimateChildren.ForEach(d => d.GotoFrame(0));
            }
        }
    }
}
