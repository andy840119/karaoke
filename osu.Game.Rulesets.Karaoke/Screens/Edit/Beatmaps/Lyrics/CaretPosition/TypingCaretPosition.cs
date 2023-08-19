// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;

public readonly struct TypingCaretPosition : ICharGapCaretPosition, IComparable<TypingCaretPosition>
{
    public TypingCaretPosition(Lyric lyric, int charGap)
    {
        if (!StringUtils.IsCharGapInRange(lyric.Text, charGap))
            throw new InvalidOperationException();

        Lyric = lyric;
        CharGap = charGap;
    }

    public Lyric Lyric { get; }

    public int CharGap { get; }

    public int CompareTo(TypingCaretPosition other)
    {
        if (Lyric != other.Lyric)
            throw new InvalidOperationException();

        return CharGap.CompareTo(other.CharGap);
    }

    public int CompareTo(IIndexCaretPosition? other)
    {
        if (other is not TypingCaretPosition typingCaretPosition)
            throw new InvalidOperationException();

        return CompareTo(typingCaretPosition);
    }
}
