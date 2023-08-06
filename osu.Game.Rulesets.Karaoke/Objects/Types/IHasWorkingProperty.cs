// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Stages;

namespace osu.Game.Rulesets.Karaoke.Objects.Types;

public interface IHasWorkingProperty<TWorkingProperty> : IHasWorkingProperty
    where TWorkingProperty : struct, Enum
{
    bool InvalidateWorkingProperty(TWorkingProperty workingProperty);

    TWorkingProperty[] GetAllInvalidWorkingProperties();
}

public interface IHasWorkingProperty
{
    void ValidateWorkingProperty(KaraokeBeatmap beatmap);

    void ValidateWorkingProperty(StageInfo stageInfo);
}
