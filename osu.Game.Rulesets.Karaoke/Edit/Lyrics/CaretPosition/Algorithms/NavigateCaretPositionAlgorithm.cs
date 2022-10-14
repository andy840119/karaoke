﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public class NavigateCaretPositionAlgorithm : CaretPositionAlgorithm<NavigateCaretPosition>
    {
        public NavigateCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        public override bool PositionMovable(NavigateCaretPosition position)
        {
            return true;
        }

        public override NavigateCaretPosition? MoveToPreviousLyric(NavigateCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetPrevious(currentPosition.Lyric);
            if (lyric == null)
                return null;

            return new NavigateCaretPosition(lyric);
        }

        public override NavigateCaretPosition? MoveToNextLyric(NavigateCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetNext(currentPosition.Lyric);
            if (lyric == null)
                return null;

            return new NavigateCaretPosition(lyric);
        }

        public override NavigateCaretPosition? MoveToFirstLyric()
        {
            var lyric = Lyrics.FirstOrDefault();
            if (lyric == null)
                return null;

            return new NavigateCaretPosition(lyric);
        }

        public override NavigateCaretPosition? MoveToLastLyric()
        {
            var lyric = Lyrics.LastOrDefault();
            if (lyric == null)
                return null;

            return new NavigateCaretPosition(lyric);
        }

        public override NavigateCaretPosition? MoveToTargetLyric(Lyric lyric) => new(lyric, CaretGenerateType.TargetLyric);
    }
}
