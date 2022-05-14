// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Sprites
{
    public class DrawableTextIndex : RightTriangle
    {
        public TextIndex.IndexState State
        {
            get => state;
            set
            {
                state = value;

                RightAngleDirection = state switch
                {
                    TextIndex.IndexState.Start => TriangleRightAngleDirection.BottomLeft,
                    TextIndex.IndexState.End => TriangleRightAngleDirection.BottomRight,
                    _ => throw new ArgumentOutOfRangeException(nameof(value))
                };
            }
        }

        private TextIndex.IndexState state;
    }
}
