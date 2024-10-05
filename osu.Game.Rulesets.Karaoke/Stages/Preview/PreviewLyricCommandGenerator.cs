// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Commands;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Stages.Preview;

public class PreviewLyricCommandGenerator : HitObjectCommandGenerator<PreviewStageInfo, Lyric>
{
    public PreviewLyricCommandGenerator(PreviewStageInfo stageInfo)
        : base(stageInfo)
    {
    }

    protected override double GeneratePreemptTime(Lyric hitObject)
    {
        return StageInfo.StageDefinition.FadingTime;
    }

    protected override IEnumerable<IStageCommand> GenerateInitialCommands(Lyric hitObject)
    {
        var elements = StageInfo.GetStageElements(hitObject);
        return elements.Select(e => e switch
        {
            PreviewLyricLayout previewLyricLayout => updateInitialTransforms(previewLyricLayout),
            PreviewStyle => throw new NotImplementedException(),
            _ => throw new NotSupportedException(),
        }).SelectMany(x => x);
    }

    private IEnumerable<IStageCommand> updateInitialTransforms(PreviewLyricLayout layout)
    {
        var definition = StageInfo.StageDefinition;

        float initialPosition = getStartPosition(definition, layout) + definition.FadingOffsetPosition;
        float startPosition = getStartPosition(definition, layout);
        float alpha = getAlpha(definition, layout);
        double duration = fadeInDuration(definition, layout);

        yield return new StageYCommand(definition.MovingInEasing, 0, duration, initialPosition, startPosition);
        yield return new StageAlphaCommand(definition.FadeInEasing, 0, duration, 0, alpha);
        yield break;

        static float getStartPosition(PreviewStageDefinition definition, PreviewLyricLayout layout)
        {
            if (layout.StartTime != 0)
            {
                return definition.LyricHeight * (definition.NumberOfLyrics - 1);
            }

            return definition.LyricHeight * layout.Timings.Count;
        }

        static float getAlpha(PreviewStageDefinition definition, PreviewLyricLayout layout)
        {
            if (layout.Timings.Any())
            {
                return definition.InactiveAlpha;
            }

            return 1;
        }

        static double fadeInDuration(PreviewStageDefinition definition, PreviewLyricLayout layout)
        {
            if (layout.StartTime != 0)
            {
                return definition.FadingTime;
            }

            return 0;
        }
    }

    protected override IEnumerable<IStageCommand> GenerateStartTimeStateCommands(Lyric hitObject)
    {
        var elements = StageInfo.GetStageElements(hitObject);
        return elements.Select(e => e switch
        {
            PreviewLyricLayout previewLyricLayout => updateStartTimeStateTransforms(previewLyricLayout),
            PreviewStyle => throw new NotImplementedException(),
            _ => throw new NotSupportedException(),
        }).SelectMany(x => x);
    }

    private IEnumerable<IStageCommand> updateStartTimeStateTransforms(PreviewLyricLayout layout)
    {
        var definition = StageInfo.StageDefinition;
        double relativeTime = layout.StartTime;

        foreach ((int line, double time) in layout.Timings)
        {
            double offsetTime = time - relativeTime;

            float position = definition.LyricHeight * line;
            float alpha = line == 0 ? 1 : definition.InactiveAlpha;
            double fadingTime = Math.Clamp(definition.ActiveTime, 0, definition.LineMovingTime);

            yield return new StageYCommand(definition.LineMovingEasing, offsetTime, offsetTime + definition.LineMovingTime, position, position);
            yield return new StageAlphaCommand(definition.ActiveEasing, offsetTime, offsetTime - fadingTime, alpha, alpha);

            relativeTime = relativeTime + offsetTime + definition.LineMovingTime;
        }
    }

    protected override IEnumerable<IStageCommand> GenerateHitStateCommands(Lyric hitObject, ArmedState state)
    {
        var elements = StageInfo.GetStageElements(hitObject);
        return elements.Select(e => e switch
        {
            PreviewLyricLayout previewLyricLayout => updateHitStateTransforms(state, previewLyricLayout),
            PreviewStyle => throw new NotImplementedException(),
            _ => throw new NotSupportedException(),
        }).SelectMany(x => x);
    }

    private IEnumerable<IStageCommand> updateHitStateTransforms(ArmedState state, PreviewLyricLayout layout)
    {
        var definition = StageInfo.StageDefinition;
        float targetPosition = -definition.FadingOffsetPosition;

        yield return new StageAlphaCommand(definition.FadeOutEasing, 0, definition.FadingTime, 1, 0);
        yield return new StageYCommand(definition.MoveOutEasing, 0, definition.FadingTime, 0, targetPosition);
    }
}
