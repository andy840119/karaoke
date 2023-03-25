﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Replays;

namespace osu.Game.Rulesets.Karaoke.UI.Position
{
    public class NotePositionCalculator
    {
        private readonly NoteInfo noteInfo;
        private readonly float columnSpacing;
        private readonly Tone offset;

        public float ColumnHeight { get; }

        public NotePositionCalculator(NoteInfo noteInfo, float columnHeight, float columnSpacing, Tone offset = new())
        {
            ColumnHeight = columnHeight;

            // todo : not sure should column can be even.
            this.noteInfo = noteInfo;
            this.columnSpacing = columnSpacing;
            this.offset = offset;
        }

        public float YPositionAt(Note note) => YPositionAt(note.Tone);

        public float YPositionAt(Tone tone) => YPositionAt(toFloat(tone));

        public float YPositionAt(KaraokeScoringAction action) => YPositionAt(action.Scale);

        public float YPositionAt(KaraokeReplayFrame frame) => YPositionAt(frame.Scale);

        public float YPositionAt(float scale) => -(columnSpacing + ColumnHeight) * Math.Clamp(scale, toFloat(MinTone), toFloat(MaxTone));

        public Tone MaxTone => noteInfo.MaxTone;

        public Tone MinTone => noteInfo.MinTone;

        private float toFloat(Tone tone)
            => tone.Scale + (tone.Half ? 0.5f : 0);
    }
}
