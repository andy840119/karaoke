// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition
{
    public class TimeTagCaretPosition : ICaretPosition
    {
        public Lyric Lyric { get; }

        public TimeTag TimeTag { get; }

        public TimeTagCaretPosition(Lyric lyric, TimeTag timeTag)
        {
            Lyric = lyric;
            TimeTag = timeTag;
        }
    }
}
