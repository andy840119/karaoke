﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Utils;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Components.ContextMenu;
using osu.Game.Rulesets.Karaoke.Edit.Singers.Rows.Components.Blueprints;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Components.Timelines.Summary.Parts;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Rows.Components
{
    public class LyricBlueprintContainer : BlueprintContainer<Lyric>
    {
        private readonly IBindableList<Lyric> bindableLyrics = new BindableList<Lyric>();

        public LyricBlueprintContainer()
        {
            bindableLyrics.BindCollectionChanged((_, b) =>
            {
                var removedLyrics = b.OldItems?.OfType<Lyric>().ToArray();
                var createdLyrics = b.NewItems?.OfType<Lyric>().ToArray();

                if (removedLyrics != null)
                {
                    foreach (var lyric in removedLyrics)
                        RemoveBlueprintFor(lyric);
                }

                if (createdLyrics != null)
                {
                    foreach (var lyric in createdLyrics)
                        AddBlueprintFor(lyric);
                }
            });
        }

        protected override IEnumerable<SelectionBlueprint<Lyric>> SortForMovement(IReadOnlyList<SelectionBlueprint<Lyric>> blueprints)
        {
            return blueprints.OrderBy(b => b.Item.LyricStartTime);
        }

        protected override Container<SelectionBlueprint<Lyric>> CreateSelectionBlueprintContainer()
        {
            return new SingerLyricSelectionBlueprintContainer { RelativeSizeAxes = Axes.Both };
        }

        protected override SelectionHandler<Lyric> CreateSelectionHandler()
        {
            return new SingerLyricSelectionHandler();
        }

        protected override SelectionBlueprint<Lyric> CreateBlueprintFor(Lyric item)
        {
            return new LyricTimelineHitObjectBlueprint(item);
        }

        protected override DragBox CreateDragBox(Action<RectangleF> performSelect)
        {
            return new SingerLyricDragBox(performSelect);
        }

        [BackgroundDependencyLoader]
        private void load(ILyricsProvider lyricsProvider)
        {
            bindableLyrics.BindTo(lyricsProvider.BindableLyrics);
        }

        protected class SingerLyricSelectionHandler : SelectionHandler<Lyric>
        {
            [Resolved]
            private EditorBeatmap beatmap { get; set; }

            [Resolved]
            private ILyricSingerChangeHandler lyricSingerChangeHandler { get; set; }

            [Resolved]
            private BindableList<Lyric> selectedLyrics { get; set; }

            protected override void OnSelectionChanged()
            {
                base.OnSelectionChanged();

                // should hide selection box if not dragging at current row.
                bool dragging = Parent.IsDragged;
                SelectionBox.FadeTo(dragging ? 1f : 0.0f);
            }

            protected override IEnumerable<MenuItem> GetContextMenuItemsForSelection(IEnumerable<SelectionBlueprint<Lyric>> selection)
            {
                var contextMenu = new SingerContextMenu(beatmap, lyricSingerChangeHandler, "", () =>
                {
                    selectedLyrics.Clear();
                });
                return contextMenu.Items;
            }

            protected override void DeleteItems(IEnumerable<Lyric> items)
            {
                // todo : remove all in the same time.
                foreach (var item in items) lyricSingerChangeHandler.Clear();

                selectedLyrics.Clear();
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                SelectedItems.BindTo(selectedLyrics);
            }
        }

        private class SingerLyricDragBox : DragBox
        {
            // the following values hold the start and end X positions of the drag box in the timeline's local space,
            // but with zoom un-applied in order to be able to compensate for positional changes
            // while the timeline is being zoomed in/out.
            private float? selectionStart;
            private float selectionEnd;

            [Resolved]
            private SingerLyricEditor editor { get; set; }

            public SingerLyricDragBox(Action<RectangleF> performSelect)
                : base(performSelect)
            {
            }

            public override bool HandleDrag(MouseButtonEvent e)
            {
                selectionStart ??= e.MouseDownPosition.X / editor.CurrentZoom;

                // only calculate end when a transition is not in progress to avoid bouncing.
                if (Precision.AlmostEquals(editor.CurrentZoom, editor.Zoom))
                    selectionEnd = e.MousePosition.X / editor.CurrentZoom;

                updateDragBoxPosition();
                return true;
            }

            public override void Hide()
            {
                base.Hide();
                selectionStart = null;
            }

            protected override Drawable CreateBox()
            {
                return new Box
                {
                    RelativeSizeAxes = Axes.Y,
                    Alpha = 0.3f
                };
            }

            private void updateDragBoxPosition()
            {
                if (selectionStart == null)
                    return;

                float rescaledStart = selectionStart.Value * editor.CurrentZoom;
                float rescaledEnd = selectionEnd * editor.CurrentZoom;

                Box.X = Math.Min(rescaledStart, rescaledEnd);
                Box.Width = Math.Abs(rescaledStart - rescaledEnd);

                var boxScreenRect = Box.ScreenSpaceDrawQuad.AABBFloat;

                // we don't care about where the hitobjects are vertically. in cases like stacking display, they may be outside the box without this adjustment.
                boxScreenRect.Y -= boxScreenRect.Height;
                boxScreenRect.Height *= 2;

                PerformSelection?.Invoke(boxScreenRect);
            }
        }

        protected class SingerLyricSelectionBlueprintContainer : Container<SelectionBlueprint<Lyric>>
        {
            protected override Container<SelectionBlueprint<Lyric>> Content { get; }

            public SingerLyricSelectionBlueprintContainer()
            {
                AddInternal(new TimelinePart<SelectionBlueprint<Lyric>>(Content = new Container<SelectionBlueprint<Lyric>> { RelativeSizeAxes = Axes.Both }) { RelativeSizeAxes = Axes.Both });
            }
        }
    }
}
