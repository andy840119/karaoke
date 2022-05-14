﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Statistics;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.Ranking
{
    public class TestSceneNotScorableGraph : OsuTestScene
    {
        private void createTest()
        {
            AddStep("create test", () =>
            {
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4Extensions.FromHex("#333")
                    },
                    new NotScorableGraph
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(600, 130)
                    }
                };
            });
        }

        [Test]
        public void TestBeatmapInfoGraph()
        {
            createTest();
        }
    }
}
