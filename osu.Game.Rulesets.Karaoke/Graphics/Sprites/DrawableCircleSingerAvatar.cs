// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Graphics.Sprites
{
    public class DrawableCircleSingerAvatar : DrawableSingerAvatar
    {
        public override ISinger Singer
        {
            get => base.Singer;
            set
            {
                base.Singer = value;
                BorderColour = SingerUtils.GetContentColour(Singer);
            }
        }

        [BackgroundDependencyLoader]
        private void load(LargeTextureStore textures)
        {
            Masking = true;
            CornerRadius = Math.Min(DrawSize.X, DrawSize.Y) / 2f;
            BorderThickness = 5;
        }
    }
}
