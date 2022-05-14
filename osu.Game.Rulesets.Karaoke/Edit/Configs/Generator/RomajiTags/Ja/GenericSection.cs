﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags.Ja;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.RomajiTags.Ja
{
    public class GenericSection : GeneratorConfigSection<JaRomajiTagGeneratorConfig>
    {
        protected override string Title => "Generic";
        private readonly LabelledSwitchButton uppercaseSwitchButton;

        public GenericSection(Bindable<JaRomajiTagGeneratorConfig> config)
            : base(config)
        {
            Children = new Drawable[]
            {
                uppercaseSwitchButton = new LabelledSwitchButton
                {
                    Label = "Uppercase",
                    Description = "Export romaji with uppercase."
                }
            };

            RegisterConfig(uppercaseSwitchButton.Current, nameof(JaRomajiTagGeneratorConfig.Uppercase));
        }
    }
}
