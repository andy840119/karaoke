﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags.Ja;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.RubyTags.Ja
{
    public class GenericSection : GeneratorConfigSection<JaRubyTagGeneratorConfig>
    {
        protected override string Title => "Generic";
        private readonly LabelledSwitchButton rubyAsKatakanaSwitchButton;
        private readonly LabelledSwitchButton enableDuplicatedRubySwitchButton;

        public GenericSection(Bindable<JaRubyTagGeneratorConfig> config)
            : base(config)
        {
            Children = new Drawable[]
            {
                rubyAsKatakanaSwitchButton = new LabelledSwitchButton
                {
                    Label = "Ruby as Katakana",
                    Description = "Ruby as Katakana."
                },
                enableDuplicatedRubySwitchButton = new LabelledSwitchButton
                {
                    Label = "Enable duplicated ruby.",
                    Description = "Enable output duplicated ruby even it's match with lyric."
                }
            };

            RegisterConfig(rubyAsKatakanaSwitchButton.Current, nameof(JaRubyTagGeneratorConfig.RubyAsKatakana));
            RegisterConfig(enableDuplicatedRubySwitchButton.Current, nameof(JaRubyTagGeneratorConfig.EnableDuplicatedRuby));
        }
    }
}
