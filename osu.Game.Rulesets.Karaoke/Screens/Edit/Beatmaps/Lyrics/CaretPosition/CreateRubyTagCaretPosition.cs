// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;

public readonly struct CreateRubyTagCaretPosition : ICharIndexCaretPosition, IComparable<CreateRubyTagCaretPosition>
{
    public CreateRubyTagCaretPosition(Lyric lyric, int charIndex)
    {
        if (!StringUtils.IsCharIndexInRange(lyric.Text, charIndex))
            throw new InvalidOperationException();

        Lyric = lyric;
        CharIndex = charIndex;
    }

    public Lyric Lyric { get; }

    public int CharIndex { get; }

    public int CompareTo(CreateRubyTagCaretPosition other)
    {
        if (Lyric != other.Lyric)
            throw new InvalidOperationException();

        return CharIndex.CompareTo(other.CharIndex);
    }

    public int CompareTo(IIndexCaretPosition? other)
    {
        if (other is not CreateRubyTagCaretPosition createRubyTagCaretPosition)
            throw new InvalidOperationException();

        return CompareTo(createRubyTagCaretPosition);
    }

    public static bool IndexInRange(Lyric lyric, int charIndex)
    {
        return StringUtils.IsCharIndexInRange(lyric.Text, charIndex);
    }
}
