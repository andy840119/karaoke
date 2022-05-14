﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Cursor;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Skinning;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    [TestFixture]
    public class TestSceneLayoutToolTip : OsuTestScene
    {
        [SetUp]
        public void SetUp()
        {
            Schedule(() =>
            {
                Child = new SkinProvidingContainer(skin)
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = toolTip = new LayoutToolTip
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre
                    }
                };
                toolTip.Show();
            });
        }

        private readonly ISkin skin = new DefaultKaraokeSkin(null);
        private LayoutToolTip toolTip;

        private void setTooltip(string testName, Action<Lyric> callBack)
        {
            AddStep(testName, () =>
            {
                var singer = new Lyric
                {
                    Text = "karaoke!"
                };
                callBack?.Invoke(singer);
                toolTip.SetContent(singer);
            });
        }

        [Test]
        public void TestDisplayToolTip()
        {
            var layouts = skin.GetConfig<KaraokeIndexLookup, IDictionary<int, string>>(KaraokeIndexLookup.Layout)?.Value;
            if (layouts == null)
                return;

            foreach ((int key, string value) in layouts)
            {
                setTooltip($"Test lyric with layout {value}", lyric =>
                {
                    // todo: should change mapping group id from the lyric.
                });
            }
        }
    }
}
