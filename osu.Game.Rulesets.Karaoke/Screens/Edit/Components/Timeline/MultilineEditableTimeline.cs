// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Timeline;

public partial class MultilineEditableTimeline : EditableTimeline
{
    //protected override IEnumerable<Drawable> CreateBlueprintContainer()
    //{
    //    yield return new MultilineEditableTimelineBlueprintContainer<Lyric>();
    //}

    public override SnapResult FindSnappedPositionAndTime(Vector2 screenSpacePosition, SnapType snapType = SnapType.All)
    {
        // todo: should provide the row playfield.
        // for able to let blueprint ablr to
        return base.FindSnappedPositionAndTime(screenSpacePosition, snapType);
    }
}
