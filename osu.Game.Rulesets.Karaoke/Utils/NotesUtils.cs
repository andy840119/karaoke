﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class NotesUtils
    {
        public static Tuple<Note, Note> SplitNote(Note note, double percentage = 0.5)
        {
            switch (percentage)
            {
                case < 0 or > 1:
                    throw new ArgumentOutOfRangeException(nameof(note));

                case 0 or 1:
                    throw new InvalidOperationException($"{nameof(percentage)} cannot be {0} or {1}.");
            }

            double firstNoteDuration = note.Duration * percentage;
            double secondNoteDuration = note.Duration * (1 - percentage);

            var firstNote = note.DeepClone();
            firstNote.DurationOffset = secondNoteDuration;

            var secondNote = note.DeepClone();
            secondNote.StartTimeOffset = note.StartTimeOffset + firstNoteDuration;

            return new Tuple<Note, Note>(firstNote, secondNote);
        }

        public static Note CombineNote(Note firstLyric, Note secondLyric)
        {
            if (firstLyric.ReferenceLyric != secondLyric.ReferenceLyric)
                throw new InvalidOperationException($"{nameof(firstLyric.ReferenceLyric)} and {nameof(secondLyric.ReferenceLyric)} should be same.");

            if (firstLyric.ReferenceTimeTagIndex != secondLyric.ReferenceTimeTagIndex)
                throw new InvalidOperationException($"{nameof(firstLyric.ReferenceTimeTagIndex)} and {nameof(secondLyric.ReferenceTimeTagIndex)} should be same.");

            var combinedLyric = firstLyric.DeepClone();
            combinedLyric.StartTimeOffset = Math.Min(firstLyric.StartTimeOffset, secondLyric.StartTimeOffset);
            combinedLyric.DurationOffset = Math.Max(firstLyric.DurationOffset, secondLyric.DurationOffset);

            return combinedLyric;
        }
    }
}
