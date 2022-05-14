﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags
{
    public class RomajiTagGeneratorSelector : GeneratorSelector<RomajiTagGenerator, RomajiTagGeneratorConfig>
    {
        public RomajiTagGeneratorSelector(KaraokeRulesetEditGeneratorConfigManager generatorConfigManager)
            : base(generatorConfigManager)
        {
            RegisterGenerator<JaRomajiTagGenerator, JaRomajiTagGeneratorConfig>(new CultureInfo(17));
            RegisterGenerator<JaRomajiTagGenerator, JaRomajiTagGeneratorConfig>(new CultureInfo(1041));
        }

        public RomajiTag[] GenerateRomajiTags(Lyric lyric)
        {
            if (lyric.Language == null)
                return null;

            if (string.IsNullOrEmpty(lyric.Text))
                return null;

            if (!Generator.TryGetValue(lyric.Language, out var generator))
                return null;

            return generator.Value.CreateRomajiTags(lyric);
        }

        protected override KaraokeRulesetEditGeneratorSetting GetGeneratorConfigSetting(CultureInfo info)
        {
            return KaraokeRulesetEditGeneratorSetting.JaRomajiTagGeneratorConfig;
        }
    }
}
