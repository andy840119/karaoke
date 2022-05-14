﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.SubInfo
{
    public abstract class SubInfo : Container
    {
        protected Lyric Lyric { get; }

        protected LocalisableString BadgeText
        {
            get => badgeText.Text;
            set => badgeText.Text = value;
        }

        protected ColourInfo BadgeColour
        {
            get => box.Colour;
            set => box.Colour = value;
        }

        private readonly Box box;
        private readonly OsuSpriteText badgeText;

        [Resolved]
        private ILyricCaretState lyricCaretState { get; set; }

        protected SubInfo(Lyric lyric)
        {
            Lyric = lyric;

            AutoSizeAxes = Axes.Both;
            Masking = true;
            CornerRadius = 3;
            Children = new Drawable[]
            {
                box = new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                badgeText = new OsuSpriteText
                {
                    Margin = new MarginPadding
                    {
                        Vertical = 2,
                        Horizontal = 5
                    },
                    Text = "Badge"
                }
            };
        }

        protected override bool OnClick(ClickEvent e)
        {
            lyricCaretState.MoveCaretToTargetPosition(Lyric);
            return base.OnClick(e);
        }
    }
}
