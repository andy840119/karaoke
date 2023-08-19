// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Utils;

public static class StringUtils
{
    public static bool IsCharIndexInRange(string text, int charIndex)
    {
        return charIndex >= 0 && charIndex < text.Length;
    }

    public static bool IsCharGapInRange(string text, int charGap)
    {
        return charGap >= 0 && charGap <= text.Length;
    }
}
