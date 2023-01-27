// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Edit;
using osu.Game.Screens.Edit.Components.Timelines.Summary.Parts;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Timeline;

public abstract partial class MultilineEditableTimelineBlueprintContainer<TRow, TItem> : EditableTimelineBlueprintContainer<TItem> where TItem : class
{
    protected readonly IBindableList<TRow> Rows = new BindableList<TRow>();

    protected MultilineEditableTimelineBlueprintContainer()
    {
        Rows.BindCollectionChanged((_, b) =>
        {
            // todo: should handle the change order event.
            var removedItems = b.OldItems?.OfType<TRow>().ToArray();
            var createdItems = b.NewItems?.OfType<TRow>().ToArray();

            if (removedItems != null)
            {
                //foreach (var item in removedItems)
                //    RemoveBlueprintFor(item);
            }

            if (createdItems != null)
            {
                //foreach (var item in createdItems)
                //    AddBlueprintFor(item);
            }
        });
    }

    protected override void OnBlueprintAdded(TItem item)
    {
        // should make the item at the first row as default.
        // UpdateDisplayRow(item, 0);
        base.OnBlueprintAdded(item);
    }

    protected void UpdateDisplayRow(TItem item, TRow row)
    {
        var selectionBlueprint = getSelectionBlueprint(item);
        if (selectionBlueprint == null)
            throw new InvalidOperationException("Blueprint not found.");

        // todo: change the position.
    }

    private SelectionBlueprint<TItem>? getSelectionBlueprint(TItem item)
        => SelectionBlueprints.FirstOrDefault(x => x.Item == item);

    protected override bool ApplySnapResult(SelectionBlueprint<TItem>[] blueprints, SnapResult result)
    {
        // todo: get the playfield or row index.
        var items = blueprints.Select(x => x.Item).ToArray();
        var playfield = result.Playfield;

        // todo: or calculate the position in here?
        // because there's no position for it.

        // todo: should be able to get the matched value from the item.

        return base.ApplySnapResult(blueprints, result) && ApplyRowOffset(blueprints, );
    }

    protected abstract TRow? GetRowFromItem(TItem item);

    // todo: need to let the change handler able to support that.
    protected abstract bool ApplyRowOffset(TItem[] items, TRow row);

    // should give the background in here?
    protected partial class MultilineEditableTimelineSelectionBlueprintContainer : EditableTimelineSelectionBlueprintContainer
    {
        protected override Container<SelectionBlueprint<TItem>> Content { get; }

        public MultilineEditableTimelineSelectionBlueprintContainer()
        {
            AddInternal(new TimelinePart<SelectionBlueprint<TItem>>(Content = new FillFlowContainer<SelectionBlueprint<TItem>> { RelativeSizeAxes = Axes.Both }) { RelativeSizeAxes = Axes.Both });
        }
    }

    // use the origin dragbox because can drag to select multiple lines at the same time.
    protected override DragBox CreateDragBox() => new();
}
