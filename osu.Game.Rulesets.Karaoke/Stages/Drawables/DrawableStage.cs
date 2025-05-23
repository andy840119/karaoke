// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Stages.Preview;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Preview;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Types;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.Karaoke.Stages.Drawables;

/// <summary>
/// Environment for execute the stage.
/// </summary>
public partial class DrawableStage : Container
{
    [Cached(typeof(IStageHitObjectRunner))]
    private readonly StageHitObjectRunner stageRunner = new();

    [Cached(typeof(IStagePlayfieldRunner))]
    private readonly StagePlayfieldRunner stagePlayfieldRunner = new();

    [Cached(typeof(IStageElementRunner))]
    private readonly StageElementRunner stageElementRunner = new();

    [BackgroundDependencyLoader]
    private void load(IReadOnlyList<Mod> mods, IBeatmap beatmap)
    {
        Container stageLayer = new Container
        {
            RelativeSizeAxes = Axes.Both,
        };

        AddInternal(stageLayer);
        stageElementRunner.UpdateStageElements(stageLayer);

        if (beatmap is not KaraokeBeatmap karaokeBeatmap)
            throw new InvalidOperationException();

        TriggerRecalculate(karaokeBeatmap, mods);
    }

    public void TriggerRecalculate(KaraokeBeatmap karaokeBeatmap, IReadOnlyList<Mod> mods)
    {
        var stageInfo = getStageInfo(mods, karaokeBeatmap);

        // fill the working property.
        if (stageInfo is IHasCalculatedProperty calculatedProperty)
            calculatedProperty.ValidateCalculatedProperty(karaokeBeatmap);

        bool scorable = karaokeBeatmap.IsScorable();

        stageRunner.OnStageInfoChanged(stageInfo, scorable, mods);
        stagePlayfieldRunner.OnStageInfoChanged(stageInfo, scorable, mods);
        stageElementRunner.OnStageInfoChanged(stageInfo, scorable, mods);
    }

    public override void Add(Drawable drawable)
    {
        base.Add(drawable);

        if (drawable is KaraokePlayfield karaokePlayfield)
            stagePlayfieldRunner.UpdatePlayfieldTransforms(karaokePlayfield);
    }

    private static StageInfo getStageInfo(IReadOnlyList<Mod> mods, KaraokeBeatmap beatmap)
    {
        // todo: get all available stages from resource provider.
        var availableStageInfos = Array.Empty<StageInfo>();

        // Get list of matched mods.
        // Return the first stage info if no stage mod is found.
        var stageMod = mods.OfType<IApplicableToStageInfo>().SingleOrDefault();
        if (stageMod == null)
            return availableStageInfos.FirstOrDefault() ?? createDefaultStageInfo(beatmap);

        // If user select a stage mod, means user want to use the specific type of stage.
        // We should find the matched stage info from the available stage infos.
        var matchedStageInfo = availableStageInfos.FirstOrDefault(x => stageMod.CanApply(x));

        // If the matched stage info is not found, then trying to create a default one.
        if (matchedStageInfo == null)
        {
            // Note that not every stage mod can create the default stage info.
            // If not possible to create, then use the default one and not override the value in the stage info.
            var newStageInfo = stageMod.CreateDefaultStageInfo(beatmap);
            if (newStageInfo == null)
            {
                return createDefaultStageInfo(beatmap);
            }

            matchedStageInfo = newStageInfo;
        }

        stageMod.ApplyToStageInfo(matchedStageInfo);
        return matchedStageInfo;
    }

    private static StageInfo createDefaultStageInfo(KaraokeBeatmap beatmap)
    {
        var config = new PreviewStageInfoGeneratorConfig();
        var generator = new PreviewStageInfoGenerator(config);

        return (PreviewStageInfo)generator.Generate(beatmap);
    }
}
