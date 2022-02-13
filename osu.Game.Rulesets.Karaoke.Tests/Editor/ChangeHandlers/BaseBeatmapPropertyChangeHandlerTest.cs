// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers
{
    public class BaseBeatmapPropertyChangeHandlerTest<TChangeHandler, TProperty> : BaseChangeHandlerTest<TChangeHandler>
        where TChangeHandler : BeatmapPropertyChangeHandler<TProperty>, new()
    {

    }
}
