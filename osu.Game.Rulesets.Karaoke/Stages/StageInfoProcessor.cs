// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Objects.Workings;
using osu.Game.Rulesets.Karaoke.Stages.Types;

namespace osu.Game.Rulesets.Karaoke.Stages;

public class StageInfoProcessor
{
    private readonly StageInfo stageInfo;
    private readonly IBeatmap beatmap;

    public StageInfoProcessor(StageInfo stageInfo, IBeatmap beatmap)
    {
        this.stageInfo = stageInfo;
        this.beatmap = beatmap;

        // should invalidate the working property here because the stage info is changed.
        beatmap.HitObjects.OfType<Lyric>().ForEach(x =>
        {
            x.InvalidateWorkingProperty(LyricWorkingProperty.Timing);
            x.InvalidateWorkingProperty(LyricWorkingProperty.EffectApplier);
        });
        beatmap.HitObjects.OfType<Note>().ForEach(x =>
        {
            x.InvalidateWorkingProperty(NoteWorkingProperty.EffectApplier);
        });
    }

    public void Process()
    {
        if (stageInfo is IHasCalculatedProperty calculatedProperty)
            calculatedProperty.ValidateCalculatedProperty(beatmap);

        // should convert to array here because validate the working property might change the start-time and the end time.
        // which will cause got the wrong item in the array.
        foreach (var hitObject in beatmap.HitObjects.OfType<IHasWorkingProperty>().ToArray())
        {
            hitObject.ValidateWorkingProperty(stageInfo);
        }
    }
}
