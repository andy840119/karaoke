// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks;

public class CheckBeatmapElementId : CheckBeatmapProperty<IDictionary<ElementId, IHasReferenceElement[]>, KaraokeHitObject>
{
    protected override string Description => "Check the referenced element id that does not contains the item";

    public override IEnumerable<IssueTemplate> PossibleTemplates => Array.Empty<IssueTemplate>();

    protected override IDictionary<ElementId, IHasReferenceElement[]> GetPropertyFromBeatmap(KaraokeBeatmap karaokeBeatmap)
    {
        return karaokeBeatmap.GetReferenceDictionary();
    }

    protected override IEnumerable<Issue> CheckProperty(IDictionary<ElementId, IHasReferenceElement[]> property)
    {
        foreach (var (elementId, referenceElements) in property)
        {
            // todo: find the element by the element id.

            // should mark as issue template not able to get the class by element id.
            // also, should link all related referenceElements.
        }

        return base.CheckProperty(property);
    }
}
