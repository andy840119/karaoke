﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Timing;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.Notes
{
    [Cached]
    public class NoteEditor : Container
    {
        public EditorNotePlayfield Playfield { get; }
        private const int columns = 9;

        [Cached(typeof(INotePositionInfo))]
        private readonly PreviewNotePositionInfo notePositionInfo = new();

        private readonly Lyric lyric;

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        public NoteEditor(Lyric lyric)
        {
            this.lyric = lyric;
            InternalChild = new Container
            {
                Name = "Content",
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    // layers below playfield
                    Playfield = new EditorNotePlayfield(columns)
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Clock = new StopClock(lyric.LyricStartTime)
                    },
                    // layers above playfield
                    new EditNoteBlueprintContainer(lyric)
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre
                    }
                }
            };
        }

        #region Disposal

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (beatmap == null)
                return;

            beatmap.HitObjectAdded -= addHitObject;
            beatmap.HitObjectRemoved -= removeHitObject;
        }

        #endregion

        [BackgroundDependencyLoader]
        private void load()
        {
            beatmap.HitObjectAdded += addHitObject;
            beatmap.HitObjectRemoved += removeHitObject;

            // add all matched notes into playfield
            var notes = beatmap.HitObjects.OfType<Note>().Where(x => x.ParentLyric == lyric).ToList();

            foreach (var note in notes) Playfield.Add(note);
        }

        private void addHitObject(HitObject hitObject)
        {
            if (hitObject is not Note note)
                return;

            if (note.ParentLyric != lyric)
                return;

            Playfield.Add(note);
        }

        private void removeHitObject(HitObject hitObject)
        {
            if (hitObject is not Note note)
                return;

            if (note.ParentLyric != lyric)
                return;

            Playfield.Remove(note);
        }

        private class PreviewNotePositionInfo : INotePositionInfo
        {
            public IBindable<NotePositionCalculator> Position { get; } = new Bindable<NotePositionCalculator>(new NotePositionCalculator(columns, 12, ScrollingNotePlayfield.COLUMN_SPACING));

            public NotePositionCalculator Calculator => Position.Value;
        }
    }
}
