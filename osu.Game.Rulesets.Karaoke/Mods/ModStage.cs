// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.Karaoke.Mods;

public abstract class ModStage<TStageInfo> : Mod, IApplicableToBeatmap
    where TStageInfo : StageInfo
{
    public sealed override ModType Type => ModType.System;

    /// <summary>
    /// Change the stage type should not affect the score.
    /// </summary>
    public override double ScoreMultiplier => 1;

    public void ApplyToBeatmap(IBeatmap beatmap)
    {
        if (beatmap is not KaraokeBeatmap karaokeBeatmap)
            throw new InvalidCastException();

        var stageInfos = karaokeBeatmap.StageInfos;
        var matchedStageInfo = stageInfos.OfType<TStageInfo>().FirstOrDefault();

        if (matchedStageInfo != null)
        {
            ApplyToCurrentStageInfo(matchedStageInfo);
        }

        // use the matched stage info as current stage info.
        // trying to create a new one if has no matched stage info.
        // it's ok to like it as null if is not able to create the default one, beatmap processor will handle that.
        karaokeBeatmap.CurrentStageInfo = matchedStageInfo ?? CreateStageInfo(karaokeBeatmap)!;
    }

    protected abstract void ApplyToCurrentStageInfo(TStageInfo stageInfo);

    protected virtual TStageInfo? CreateStageInfo(KaraokeBeatmap beatmap)
    {
        return null;
    }
}
