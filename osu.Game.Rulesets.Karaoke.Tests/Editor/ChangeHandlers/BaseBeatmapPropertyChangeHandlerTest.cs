// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers
{
    public abstract class BaseBeatmapPropertyChangeHandlerTest<TChangeHandler, TProperty> : BaseChangeHandlerTest<TChangeHandler>
        where TChangeHandler : BeatmapPropertyChangeHandler<TProperty>, new()
    {
        protected void PrepareBeatmapProperty(TProperty property)
            => PrepareBeatmapProperties(new[] { property });

        protected void PrepareBeatmapProperties(IEnumerable<TProperty> listOfProperties)
        {
            AddStep("Prepare testing beatmap properties", () =>
            {
                // todo: add the properties into karaoke beatmap.
            });
        }
    }
}
