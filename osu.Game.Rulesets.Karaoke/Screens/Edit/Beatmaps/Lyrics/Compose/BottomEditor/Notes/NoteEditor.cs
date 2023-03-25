﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Timing;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.UI;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.BottomEditor.Notes
{
    [Cached]
    public partial class NoteEditor : Container
    {
        [Cached(typeof(INotePositionInfo))]
        private readonly PreviewNotePositionInfo notePositionInfo;

        [Resolved, AllowNull]
        private EditorBeatmap beatmap { get; set; }

        private readonly IBindable<Lyric?> bindableFocusedLyric = new Bindable<Lyric?>();

        [Cached]
        private readonly BindableList<Note> bindableNotes = new();

        [Cached(typeof(Playfield))]
        public EditorNotePlayfield Playfield { get; }

        public NoteEditor()
        {
            var karaokeBeatmap = EditorBeatmapUtils.GetPlayableBeatmap(beatmap);
            var noteInfo = karaokeBeatmap.NoteInfo;
            notePositionInfo = new PreviewNotePositionInfo(noteInfo);

            InternalChild = new Container
            {
                Name = "Content",
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    // layers below playfield
                    Playfield = new EditorNotePlayfield(noteInfo)
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    },
                    // layers above playfield
                    new EditNoteBlueprintContainer
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    },
                }
            };

            bindableFocusedLyric.BindValueChanged(e =>
            {
                bindableNotes.Clear();

                var lyric = e.NewValue;
                if (lyric == null)
                    return;

                Playfield.Clock = new StopClock(lyric.LyricStartTime);

                // add all matched notes into playfield
                var notes = EditorBeatmapUtils.GetNotesByLyric(beatmap, lyric);
                bindableNotes.AddRange(notes);
            });

            bindableNotes.BindCollectionChanged((_, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        Debug.Assert(args.NewItems != null);

                        foreach (var obj in args.NewItems.OfType<Note>())
                            Playfield.Add(obj);

                        break;

                    case NotifyCollectionChangedAction.Remove:
                        Debug.Assert(args.OldItems != null);

                        foreach (var obj in args.OldItems.OfType<Note>())
                            Playfield.Remove(obj);

                        break;
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load(ILyricCaretState lyricCaretState)
        {
            bindableFocusedLyric.BindTo(lyricCaretState.BindableFocusedLyric);

            beatmap.HitObjectAdded += addHitObject;
            beatmap.HitObjectRemoved += removeHitObject;
        }

        private void addHitObject(HitObject hitObject)
        {
            if (hitObject is not Note note)
                return;

            if (note.ReferenceLyric != bindableFocusedLyric.Value)
                return;

            bindableNotes.Add(note);
        }

        private void removeHitObject(HitObject hitObject)
        {
            if (hitObject is not Note note)
                return;

            if (note.ReferenceLyric != bindableFocusedLyric.Value)
                return;

            bindableNotes.Remove(note);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            beatmap.HitObjectAdded -= addHitObject;
            beatmap.HitObjectRemoved -= removeHitObject;
        }

        private class PreviewNotePositionInfo : INotePositionInfo
        {
            public PreviewNotePositionInfo(NoteInfo noteInfo)
            {
                Position = new Bindable<NotePositionCalculator>(new NotePositionCalculator(noteInfo, 12, ScrollingNotePlayfield.COLUMN_SPACING));
            }

            public IBindable<NotePositionCalculator> Position { get; }

            public NotePositionCalculator Calculator => Position.Value;
        }
    }
}
