// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils;

public class StringUtilsTest
{
    [TestCase("123", 0, true)]
    [TestCase("123", 2, true)]
    [TestCase("123", 3, false)]
    [TestCase("123", -1, false)]
    [TestCase("", 0, false)]
    public void TestIsCharIndexInRange(string text, int charIndex, bool expected)
    {
        bool actual = StringUtils.IsCharIndexInRange(text, charIndex);
        Assert.AreEqual(expected, actual);
    }

    [TestCase("123", 0, true)]
    [TestCase("123", 3, true)]
    [TestCase("123", 4, false)]
    [TestCase("123", -1, false)]
    [TestCase("", 0, true)]
    public void TestIsCharGapInRange(string text, int charGap, bool expected)
    {
        bool actual = StringUtils.IsCharGapInRange(text, charGap);
        Assert.AreEqual(expected, actual);
    }
}
