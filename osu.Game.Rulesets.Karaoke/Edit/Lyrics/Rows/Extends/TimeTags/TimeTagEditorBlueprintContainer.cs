﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Utils;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Components.Timelines.Summary.Parts;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.TimeTags
{
    public class TimeTagEditorBlueprintContainer : LyricPropertyBlueprintContainer<TimeTag>
    {
        [Resolved(CanBeNull = true)]
        private TimeTagEditor timeline { get; set; }

        [Resolved]
        private EditorClock editorClock { get; set; }

        [Resolved]
        private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; }

        [UsedImplicitly]
        private readonly BindableList<TimeTag> timeTags;

        protected readonly Lyric Lyric;

        public TimeTagEditorBlueprintContainer(Lyric lyric)
        {
            Lyric = lyric;
            timeTags = lyric.TimeTagsBindable.GetBoundCopy();
        }

        [BackgroundDependencyLoader]
        private void load(ITimeTagModeState timeTagModeState)
        {
            // Add time-tag into blueprint container
            RegisterBindable(timeTags);
        }

        protected override IEnumerable<SelectionBlueprint<TimeTag>> SortForMovement(IReadOnlyList<SelectionBlueprint<TimeTag>> blueprints)
            => blueprints.OrderBy(b => b.Item.Index);

        protected override bool ApplySnapResult(SelectionBlueprint<TimeTag>[] blueprints, SnapResult result)
        {
            if (!base.ApplySnapResult(blueprints, result))
                return false;

            if (result.Time == null)
                return false;

            var timeTagBlueprints = blueprints.OfType<TimeTagEditorHitObjectBlueprint>().ToArray();
            var firstDragBlueprint = timeTagBlueprints.FirstOrDefault();
            if (firstDragBlueprint == null)
                return false;

            double offset = result.Time.Value - timeline.GetPreviewTime(firstDragBlueprint.Item);
            if (offset == 0)
                return false;

            // todo : should not save separately.
            foreach (var blueprint in timeTagBlueprints)
            {
                var timeTag = blueprint.Item;
                timeTag.Time = timeline.GetPreviewTime(timeTag) + offset;
            }

            return true;
        }

        /// <summary>
        /// Commit time-tag time.
        /// </summary>
        protected override void DragOperationCompleted()
        {
            var processedTimeTags = SelectionBlueprints.Where(x => x.State == SelectionState.Selected).Select(x => x.Item);

            // todo : should change together.
            foreach (var timeTag in processedTimeTags)
            {
                if (timeTag.Time.HasValue)
                    lyricTimeTagsChangeHandler.SetTimeTagTime(timeTag, timeTag.Time.Value);
            }
        }

        protected override Container<SelectionBlueprint<TimeTag>> CreateSelectionBlueprintContainer()
            => new TimeTagEditorSelectionBlueprintContainer { RelativeSizeAxes = Axes.Both };

        protected override SelectionHandler<TimeTag> CreateSelectionHandler()
            => new TimeTagEditorSelectionHandler();

        protected override SelectionBlueprint<TimeTag> CreateBlueprintFor(TimeTag item)
            => new TimeTagEditorHitObjectBlueprint(item);

        protected override DragBox CreateDragBox(Action<RectangleF> performSelect) => new TimelineDragBox(performSelect);

        protected override bool OnClick(ClickEvent e)
        {
            base.OnClick(e);

            // skip if already have selected blueprint.
            if (ClickedBlueprint != null)
                return true;

            // navigation to target time.
            var navigationTime = timeline.FindSnappedPositionAndTime(e.ScreenSpaceMousePosition);
            if (navigationTime.Time == null)
                return false;

            editorClock.SeekSmoothlyTo(navigationTime.Time.Value);
            return true;
        }

        protected class TimeTagEditorSelectionHandler : LyricPropertySelectionHandler<TimeTag>
        {
            [Resolved]
            private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; }

            [BackgroundDependencyLoader]
            private void load(ITimeTagModeState timeTagModeState)
            {
                SelectedItems.BindTo(timeTagModeState.SelectedItems);
            }

            // for now we always allow movement. snapping is provided by the Timeline's "distance" snap implementation
            public override bool HandleMovement(MoveSelectionEvent<TimeTag> moveEvent) => true;

            protected override void DeleteItems(IEnumerable<TimeTag> items)
            {
                lyricTimeTagsChangeHandler.RemoveRange(items);
            }

            protected override IEnumerable<MenuItem> GetContextMenuItemsForSelection(IEnumerable<SelectionBlueprint<TimeTag>> selection)
            {
                var timeTags = selection.Select(x => x.Item).ToArray();

                if (timeTags.Any(x => x.Time != null))
                {
                    return new[]
                    {
                        new OsuMenuItem("Clear time", MenuItemType.Standard, () =>
                        {
                            timeTags.ForEach(x => x.Time = null);

                            // todo : should re-calculate all preview position because some time-tag without position might be affected.
                        })
                    };
                }

                return base.GetContextMenuItemsForSelection(selection);
            }
        }

        private class TimelineDragBox : DragBox
        {
            // the following values hold the start and end X positions of the drag box in the timeline's local space,
            // but with zoom un-applied in order to be able to compensate for positional changes
            // while the timeline is being zoomed in/out.
            private float? selectionStart;
            private float selectionEnd;

            [Resolved]
            private TimeTagEditor timeline { get; set; }

            public TimelineDragBox(Action<RectangleF> performSelect)
                : base(performSelect)
            {
            }

            protected override Drawable CreateBox() => new Box
            {
                RelativeSizeAxes = Axes.Y,
                Alpha = 0.3f
            };

            public override bool HandleDrag(MouseButtonEvent e)
            {
                selectionStart ??= e.MouseDownPosition.X / timeline.CurrentZoom;

                // only calculate end when a transition is not in progress to avoid bouncing.
                if (Precision.AlmostEquals(timeline.CurrentZoom, timeline.Zoom))
                    selectionEnd = e.MousePosition.X / timeline.CurrentZoom;

                updateDragBoxPosition();
                return true;
            }

            private void updateDragBoxPosition()
            {
                if (selectionStart == null)
                    return;

                float rescaledStart = selectionStart.Value * timeline.CurrentZoom;
                float rescaledEnd = selectionEnd * timeline.CurrentZoom;

                Box.X = Math.Min(rescaledStart, rescaledEnd);
                Box.Width = Math.Abs(rescaledStart - rescaledEnd);

                var boxScreenRect = Box.ScreenSpaceDrawQuad.AABBFloat;

                // we don't care about where the hitobjects are vertically. in cases like stacking display, they may be outside the box without this adjustment.
                boxScreenRect.Y -= boxScreenRect.Height;
                boxScreenRect.Height *= 2;

                PerformSelection?.Invoke(boxScreenRect);
            }

            public override void Hide()
            {
                base.Hide();
                selectionStart = null;
            }
        }

        protected class TimeTagEditorSelectionBlueprintContainer : Container<SelectionBlueprint<TimeTag>>
        {
            protected override Container<SelectionBlueprint<TimeTag>> Content { get; }

            public TimeTagEditorSelectionBlueprintContainer()
            {
                AddInternal(new TimelinePart<SelectionBlueprint<TimeTag>>(Content = new TimeTagOrderedSelectionContainer { RelativeSizeAxes = Axes.Both }) { RelativeSizeAxes = Axes.Both });
            }
        }
    }
}
