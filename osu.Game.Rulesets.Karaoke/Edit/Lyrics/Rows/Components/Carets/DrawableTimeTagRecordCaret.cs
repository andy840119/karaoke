﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Carets
{
    public class DrawableTimeTagRecordCaret : DrawableCaret<TimeTagCaretPosition>
    {
        private const float triangle_width = 8;

        private readonly DrawableTextIndex drawableTextIndex;

        [Resolved]
        private OsuColour colours { get; set; }

        [Resolved]
        private EditorKaraokeSpriteText karaokeSpriteText { get; set; }

        public DrawableTimeTagRecordCaret(bool preview)
            : base(preview)
        {
            AutoSizeAxes = Axes.Both;

            InternalChild = drawableTextIndex = new DrawableTextIndex
            {
                Name = "Text index",
                Size = new Vector2(triangle_width),
                Alpha = preview ? 0.5f : 1
            };
        }

        protected override void Apply(TimeTagCaretPosition caret)
        {
            var timeTag = caret.TimeTag;
            var textIndex = timeTag.Index;
            this.MoveTo(karaokeSpriteText.GetTimeTagPosition(timeTag), Preview ? 0 : 100, Easing.OutCubic);
            Origin = TextIndexUtils.GetValueByState(textIndex, Anchor.BottomLeft, Anchor.BottomRight);

            drawableTextIndex.State = textIndex.State;
            drawableTextIndex.Colour = colours.GetRecordingTimeTagCaretColour(timeTag);
        }
    }
}
