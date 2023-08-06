// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

public interface IBeatmapStagesChangeHandler : ITypeAutoGenerateChangeHandler<StageInfo>
{
    LocalisableString? GetGeneratorNotSupportedMessage<TStageInfo>() where TStageInfo : StageInfo;

    void Remove<TStageInfo>() where TStageInfo : StageInfo;
}
