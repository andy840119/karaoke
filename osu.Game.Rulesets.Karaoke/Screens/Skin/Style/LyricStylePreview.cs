﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Style
{
    internal class LyricStylePreview : Container
    {
        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider, StyleManager manager)
        {
            Masking = true;
            CornerRadius = 15;
            FillMode = FillMode.Fit;
            FillAspectRatio = 1.25f;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = colourProvider.Background1
                },
                new PreviewDrawableLyricLine(createDefaultLyricLine())
            };
        }

        private Lyric createDefaultLyricLine()
        {
            double startTime = Time.Current;
            const double duration = 1000000;

            return new Lyric
            {
                StartTime = startTime,
                Duration = duration,
                Text = "カラオケ！",
                TimeTags = new List<TimeTag>
                {
                    new(new TextIndex(0), startTime + 500),
                    new(new TextIndex(1), startTime + 600),
                    new(new TextIndex(2), startTime + 1000),
                    new(new TextIndex(3), startTime + 1500),
                    new(new TextIndex(4), startTime + 2000)
                },
                RubyTags = new[]
                {
                    new RubyTag
                    {
                        StartIndex = 0,
                        EndIndex = 1,
                        Text = "か"
                    },
                    new RubyTag
                    {
                        StartIndex = 2,
                        EndIndex = 3,
                        Text = "お"
                    }
                },
                RomajiTags = new[]
                {
                    new RomajiTag
                    {
                        StartIndex = 1,
                        EndIndex = 2,
                        Text = "ra"
                    },
                    new RomajiTag
                    {
                        StartIndex = 3,
                        EndIndex = 4,
                        Text = "ke"
                    }
                }
            };
        }

        private class PreviewDrawableLyricLine : DrawableLyric
        {
            public PreviewDrawableLyricLine(Lyric hitObject)
                : base(hitObject)
            {
            }
        }
    }
}
