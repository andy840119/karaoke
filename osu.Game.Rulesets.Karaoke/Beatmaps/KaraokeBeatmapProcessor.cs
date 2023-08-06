// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Stages;
using osu.Game.Rulesets.Karaoke.Stages.Preview;

namespace osu.Game.Rulesets.Karaoke.Beatmaps;

public class KaraokeBeatmapProcessor : BeatmapProcessor
{
    public new KaraokeBeatmap Beatmap => (KaraokeBeatmap)base.Beatmap;

    // todo: stage info processor should have it's own process way.
    private readonly StageInfoProcessor stageInfoProcessor;

    public KaraokeBeatmapProcessor(IBeatmap beatmap)
        : base(beatmap)
    {
        var finalStageInfo = Beatmap.CurrentStageInfo ?? getWorkingStage() ?? createDefaultWorkingStage();
        Beatmap.CurrentStageInfo = finalStageInfo;
        stageInfoProcessor = new StageInfoProcessor(finalStageInfo, beatmap);

        StageInfo? getWorkingStage()
            => Beatmap.StageInfos.FirstOrDefault();

        StageInfo createDefaultWorkingStage() => new PreviewStageInfo();
    }

    public override void PreProcess()
    {
        base.PreProcess();
        stageInfoProcessor.Process();

        applyInvalidProperty(Beatmap);
    }

    private void applyInvalidProperty(KaraokeBeatmap beatmap)
    {
        // should convert to array here because validate the working property might change the start-time and the end time.
        // which will cause got the wrong item in the array.
        foreach (var hitObject in beatmap.HitObjects.OfType<IHasWorkingProperty>().ToArray())
        {
            hitObject.ValidateWorkingProperty(beatmap);
        }
    }
}
