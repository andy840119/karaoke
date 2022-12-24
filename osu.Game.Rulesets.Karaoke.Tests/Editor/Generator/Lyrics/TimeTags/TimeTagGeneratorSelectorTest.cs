﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.TimeTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Lyrics.TimeTags
{
    public class TimeTagGeneratorSelectorTest : BaseLyricGeneratorSelectorTest<TimeTagGeneratorSelector, TimeTag[]>
    {
        [TestCase(17, "か", new[] { "[0,start]:", "[0,end]:" })] // Japanese
        [TestCase(1041, "か", new[] { "[0,start]:", "[0,end]:" })] // Japanese
        [TestCase(1028, "喵", new[] { "[0,start]:" })] // Chinese
        [TestCase(3081, "hello", new string[] { })] // English
        public void TestGenerate(int lcid, string text, string[] expectedTimeTags)
        {
            var selector = CreateSelector();
            var lyric = new Lyric
            {
                Language = new CultureInfo(lcid),
                Text = text,
            };

            var expected = TestCaseTagHelper.ParseTimeTags(expectedTimeTags);
            CheckGenerateResult(lyric, expected, selector);
        }

        protected override void AssertEqual(TimeTag[] expected, TimeTag[] actual)
        {
            TimeTagAssert.ArePropertyEqual(expected, actual);
        }
    }
}
