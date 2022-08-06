﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
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

            double firstNoteStartTime = note.StartTime;
            double firstNoteDuration = note.Duration * percentage;

            double secondNoteStartTime = firstNoteStartTime + firstNoteDuration;
            double secondNoteDuration = note.Duration * (1 - percentage);

            var firstNote = NoteUtils.CopyByTime(note, firstNoteStartTime, firstNoteDuration);
            var secondNote = NoteUtils.CopyByTime(note, secondNoteStartTime, secondNoteDuration);

            return new Tuple<Note, Note>(firstNote, secondNote);
        }

        public static Note CombineNote(Note firstLyric, Note secondLyric)
        {
            if (firstLyric.ReferenceLyric != secondLyric.ReferenceLyric)
                throw new InvalidOperationException($"{nameof(firstLyric.ReferenceLyric)} and {nameof(secondLyric.ReferenceLyric)} should be same.");

            if (firstLyric.ReferenceTimeTagIndex != secondLyric.ReferenceTimeTagIndex)
                throw new InvalidOperationException($"{nameof(firstLyric.ReferenceTimeTagIndex)} and {nameof(secondLyric.ReferenceTimeTagIndex)} should be same.");

            double startTime = Math.Min(firstLyric.StartTime, secondLyric.StartTime);
            double endTime = Math.Max(firstLyric.EndTime, secondLyric.EndTime);

            return NoteUtils.CopyByTime(firstLyric, startTime, endTime - startTime);
        }

        public static IEnumerable<Note> Sort(IEnumerable<Note> notes)
            => notes.OrderBy(x => x.StartTime).ThenBy(x => x.EndIndex);

        public static Tuple<IEnumerable<Note>, IEnumerable<Note>> SeparateNoteByTime(Note[] notes, double splitTime)
        {
            var sortedNotes = Sort(notes);
            int splitIndex = sortedNotes.ToList().FindIndex(x => x.EndTime > splitTime);

            return splitIndex == -1
                ? new Tuple<IEnumerable<Note>, IEnumerable<Note>>(notes, new Note[] { })
                : new Tuple<IEnumerable<Note>, IEnumerable<Note>>(notes[..splitIndex], notes[splitIndex..]);
        }
    }
}
