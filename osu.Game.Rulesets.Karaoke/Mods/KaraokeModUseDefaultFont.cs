// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Commands;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.Karaoke.Mods;

// public class KaraokeModUseDefaultFont: ModStageHitObjectCommand<Lyric>
public class KaraokeModUseDefaultFont: Mod, IApplicableToStageInfo
{
    public override string Name => "Override font";

    public override LocalisableString Description => "Use the font from the ruleset configuration.";

    public override string Acronym => "OF";

    public override bool CanApply(StageInfo stageInfo)
    {
        throw new System.NotImplementedException();
    }

    public void GetConfigiration(KaraokeRulesetConfigManager configManager)
    {
        configManager.BindWith(KaraokeRulesetSetting.MainFont, mainFontUsageBindable);
        configManager.BindWith(KaraokeRulesetSetting.RubyFont, rubyFontUsageBindable);
        configManager.BindWith(KaraokeRulesetSetting.RubyMargin, rubyMarginBindable);
        configManager.BindWith(KaraokeRulesetSetting.RomanisationFont, romanisationFontUsageBindable);
        configManager.BindWith(KaraokeRulesetSetting.RomanisationMargin, romanisationMarginBindable);
        configManager.BindWith(KaraokeRulesetSetting.TranslationFont, translationFontUsageBindable);
    }

    protected override IEnumerable<IStageCommand> PostProcessInitialCommands(IEnumerable<IStageCommand> commands)
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerable<IStageCommand> PostProcessStartTimeStateCommands(IEnumerable<IStageCommand> commands)
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerable<IStageCommand> PostProcessHitStateCommands(IEnumerable<IStageCommand> commands)
    {
        throw new System.NotImplementedException();
    }
}
