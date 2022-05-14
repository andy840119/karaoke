﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Languages;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.Languages
{
    public class LanguageDetectorConfigPopover : GeneratorConfigPopover<LanguageDetectorConfig>
    {
        protected override KaraokeRulesetEditGeneratorSetting Config => KaraokeRulesetEditGeneratorSetting.LanguageDetectorConfig;

        protected override GeneratorConfigSection[] CreateConfigSection(Bindable<LanguageDetectorConfig> current)
        {
            return new GeneratorConfigSection[]
            {
                new AcceptLanguagesSection(current)
            };
        }
    }
}
