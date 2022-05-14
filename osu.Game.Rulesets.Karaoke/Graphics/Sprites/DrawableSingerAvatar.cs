﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;

namespace osu.Game.Rulesets.Karaoke.Graphics.Sprites
{
    public class DrawableSingerAvatar : Container
    {
        public virtual ISinger Singer
        {
            get;
            set;
            // todo : update texture.
        }

        [BackgroundDependencyLoader]
        private void load(LargeTextureStore textures)
        {
            if (textures == null)
                throw new ArgumentNullException(nameof(textures));

            // todo : get real texture from beatmap
            var texture = textures.Get(@"Online/avatar-guest");

            Add(new Sprite
            {
                RelativeSizeAxes = Axes.Both,
                Texture = texture,
                FillMode = FillMode.Fit,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            });
        }
    }
}
