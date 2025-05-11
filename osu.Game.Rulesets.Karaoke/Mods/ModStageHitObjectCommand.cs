// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Stages.Commands;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Mods;

// note: might not have base interface for the mod that adjust the single property.
// because might be possible to adjsut multiple properties at the same time.
public abstract class ModStageHitObjectCommand<THitObject> : Mod, IApplicableToStageHitObjectCommand
    where THitObject : HitObject
{
    public sealed override ModType Type => ModType.Conversion;

    /// <summary>
    /// Change the stage type should not affect the score.
    /// </summary>
    public override double ScoreMultiplier => 1;

    public override Type[] IncompatibleMods => new[] { typeof(ModStageHitObjectCommand<THitObject>) }.Except(new[] { GetType() }).ToArray();

    public abstract bool CanApply(StageInfo stageInfo);

    public IEnumerable<IStageCommand> PostProcessInitialCommands(HitObject hitObject, IEnumerable<IStageCommand> commands)
        => hitObject is THitObject ? PostProcessInitialCommands(commands) : commands;

    public IEnumerable<IStageCommand> PostProcessStartTimeStateCommands(HitObject hitObject, IEnumerable<IStageCommand> commands)
        => hitObject is THitObject ? PostProcessStartTimeStateCommands(commands) : commands;

    public IEnumerable<IStageCommand> PostProcessHitStateCommands(HitObject hitObject, IEnumerable<IStageCommand> commands)
        => hitObject is THitObject ? PostProcessHitStateCommands(commands) : commands;

    protected abstract IEnumerable<IStageCommand> PostProcessInitialCommands(IEnumerable<IStageCommand> commands);

    protected abstract IEnumerable<IStageCommand> PostProcessStartTimeStateCommands(IEnumerable<IStageCommand> commands);

    protected abstract IEnumerable<IStageCommand> PostProcessHitStateCommands(IEnumerable<IStageCommand> commands);
}
