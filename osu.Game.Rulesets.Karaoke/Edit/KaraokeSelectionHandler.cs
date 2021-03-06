﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Components.ContextMenu;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Notes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit.Compose.Components;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class KaraokeSelectionHandler : EditorSelectionHandler
    {
        [Resolved]
        private INotePositionInfo notePositionInfo { get; set; }

        [Resolved]
        private ISkinSource source { get; set; }

        [Resolved]
        private HitObjectComposer composer { get; set; }

        [Resolved]
        private NoteManager noteManager { get; set; }

        [Resolved]
        private LyricManager lyricManager { get; set; }

        protected ScrollingNotePlayfield NotePlayfield => ((KaraokeHitObjectComposer)composer).Playfield.NotePlayfield;

        protected override IEnumerable<MenuItem> GetContextMenuItemsForSelection(IEnumerable<SelectionBlueprint<HitObject>> selection)
        {
            if (selection.All(x => x is LyricSelectionBlueprint))
            {
                var selectedObject = EditorBeatmap.SelectedHitObjects.Cast<Lyric>().OrderBy(x => x.StartTime).ToList();
                return new[]
                {
                    createLayoutMenuItem(selectedObject),
                    createSingerMenuItem(selectedObject)
                };
            }

            if (EditorBeatmap.SelectedHitObjects.All(x => x is Note)
                && EditorBeatmap.SelectedHitObjects.Count > 1)
            {
                var menu = new List<MenuItem>();
                var selectedObject = EditorBeatmap.SelectedHitObjects.Cast<Note>().OrderBy(x => x.StartTime);

                // Set multi note display property
                menu.Add(createMultiNoteDisplayPropertyMenuItem(selectedObject));

                // Combine multi note if they has same start and end index.
                var firstObject = selectedObject.FirstOrDefault();
                if (firstObject != null && selectedObject.All(x => x.StartIndex == firstObject.StartIndex && x.EndIndex == firstObject.EndIndex))
                    menu.Add(createCombineNoteMenuItem(selectedObject));

                return menu;
            }

            return new List<MenuItem>();
        }

        private MenuItem createMultiNoteDisplayPropertyMenuItem(IEnumerable<Note> selectedObject)
        {
            var display = selectedObject.Count(x => x.Display) >= selectedObject.Count(x => !x.Display);
            var displayText = display ? "Hide" : "Show";
            return new OsuMenuItem($"{displayText} {selectedObject.Count()} notes.", display ? MenuItemType.Destructive : MenuItemType.Standard,
                () =>
                {
                    var selectedNotes = SelectedBlueprints.Select(x => x.Item).OfType<Note>().ToList();
                    noteManager.ChangeDisplay(selectedNotes, !display);
                });
        }

        private MenuItem createCombineNoteMenuItem(IEnumerable<Note> selectedObject)
        {
            return new OsuMenuItem("Combine", MenuItemType.Standard, () =>
            {
                noteManager.CombineNote(selectedObject.ToList());
            });
        }

        private MenuItem createLayoutMenuItem(List<Lyric> lyrics)
        {
            var layoutDictionary = source.GetConfig<KaraokeIndexLookup, IDictionary<int, string>>(KaraokeIndexLookup.Layout)?.Value;
            var selectedLayoutIndexes = lyrics.Select(x => x.LayoutIndex).Distinct().ToList();
            var selectedLayoutIndex = selectedLayoutIndexes.Count == 1 ? selectedLayoutIndexes.FirstOrDefault() : -1;

            return new OsuMenuItem("Layout")
            {
                Items = layoutDictionary?.Select(x => new TernaryStateToggleMenuItem(x.Value, selectedLayoutIndex == x.Key ? MenuItemType.Highlighted : MenuItemType.Standard, state =>
                {
                    if (state != TernaryState.True)
                        return;

                    lyricManager.ChangeLayout(lyrics, x.Key);
                })).ToArray()
            };
        }

        private MenuItem createSingerMenuItem(List<Lyric> lyrics)
        {
            return new SingerContextMenu(lyricManager, lyrics, "Singer");
        }

        public override bool HandleMovement(MoveSelectionEvent<HitObject> moveEvent)
        {
            // Only note can be moved.
            if (!(moveEvent.Blueprint is NoteSelectionBlueprint noteSelectionBlueprint))
                return false;

            var lastTone = noteSelectionBlueprint.HitObject.Tone;
            performColumnMovement(lastTone, moveEvent);

            return true;
        }

        private void performColumnMovement(Tone lastTone, MoveSelectionEvent<HitObject> moveEvent)
        {
            if (!(moveEvent.Blueprint is NoteSelectionBlueprint))
                return;

            var calculator = notePositionInfo.Calculator;

            // get center position
            var screenSpacePosition = moveEvent.Blueprint.ScreenSpaceSelectionPoint + moveEvent.ScreenSpaceDelta;
            var position = NotePlayfield.ToLocalSpace(screenSpacePosition);
            var centerPosition = new Vector2(position.X, position.Y - NotePlayfield.Height / 2);

            // get delta position
            var lastCenterPosition = calculator.YPositionAt(lastTone);
            var delta = centerPosition.Y - lastCenterPosition;

            // get delta tone.
            var deltaTone = new Tone();
            const float trigger_height = ScrollingNotePlayfield.COLUMN_SPACING + DefaultColumnBackground.COLUMN_HEIGHT;

            if (delta > trigger_height)
                deltaTone = -new Tone { Half = true };
            else if (delta < 0)
                deltaTone = new Tone { Half = true };

            if (deltaTone == 0)
                return;

            foreach (var note in EditorBeatmap.SelectedHitObjects.OfType<Note>())
            {
                if (note.Tone >= calculator.MaxTone && deltaTone > 0)
                    continue;
                if (note.Tone <= calculator.MinTone && deltaTone < 0)
                    continue;

                note.Tone += deltaTone;

                //Change all note to visible
                note.Display = true;
            }
        }
    }
}
