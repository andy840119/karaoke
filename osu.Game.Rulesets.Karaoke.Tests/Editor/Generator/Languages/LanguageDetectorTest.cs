﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Languages;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Languages
{
    [TestFixture]
    public class LanguageDetectorTest : BaseDetectorTest<LanguageDetector, CultureInfo, LanguageDetectorConfig>
    {
        [TestCase("花火大会", true)]
        [TestCase("", false)] // will not able to detect the language if lyric is empty.
        [TestCase("   ", false)]
        [TestCase(null, false)]
        public void TestCanDetect(string text, bool canDetect)
        {
            var config = GeneratorConfig();
            CheckCanDetect(text, canDetect, config);
        }

        [TestCase("花火大会", "zh-CN")]
        [TestCase("花火大會", "zh-TW")]
        [TestCase("Testing", "en")]
        [TestCase("ハナビ", "ja")]
        [TestCase("はなび", "ja")]
        public void TestDetect(string text, string language)
        {
            var config = GeneratorConfig();
            var expected = new CultureInfo(language);
            CheckDetectResult(text, expected, config);
        }

        protected override void AssertEqual(CultureInfo expected, CultureInfo actual)
        {
            Assert.AreEqual(expected, actual);
        }
    }
}
