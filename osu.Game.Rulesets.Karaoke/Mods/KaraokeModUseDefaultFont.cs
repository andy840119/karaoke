// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Commands;
using osu.Game.Rulesets.Karaoke.Stages.Infos;

namespace osu.Game.Rulesets.Karaoke.Mods;


public class KaraokeModUseDefaultFont: ModStageHitObjectCommand<Lyric>
{
    public override string Name { get; } = "";

    public override LocalisableString Description { get; }

    public override string Acronym { get; } = "";

    public override bool CanApply(StageInfo stageInfo)
    {
        throw new System.NotImplementedException();
    }

    public void GetConfigiration(KaraokeRulesetConfigManager configManager)
    {
        config.BindWith(KaraokeRulesetSetting.MainFont, mainFontUsageBindable);
        config.BindWith(KaraokeRulesetSetting.RubyFont, rubyFontUsageBindable);
        config.BindWith(KaraokeRulesetSetting.RubyMargin, rubyMarginBindable);
        config.BindWith(KaraokeRulesetSetting.RomanisationFont, romanisationFontUsageBindable);
        config.BindWith(KaraokeRulesetSetting.RomanisationMargin, romanisationMarginBindable);
        config.BindWith(KaraokeRulesetSetting.TranslationFont, translationFontUsageBindable);
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
