﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.RomajiTags
{
    public abstract class BaseRomajiTagGeneratorTest<TRomajiTagGenerator, TConfig> : BaseGeneratorTest<TRomajiTagGenerator, RomajiTag[], TConfig>
        where TRomajiTagGenerator : RomajiTagGenerator<TConfig> where TConfig : RomajiTagGeneratorConfig, new()
    {
        protected void CheckGenerateResult(string text, string[] expectedRubies, TConfig config)
        {
            var expected = TestCaseTagHelper.ParseRomajiTags(expectedRubies);
            CheckGenerateResult(text, expected, config);
        }

        protected override void AssertEqual(RomajiTag[] expected, RomajiTag[] actual)
        {
            TextTagAssert.ArePropertyEqual(expected, actual);
        }
    }
}