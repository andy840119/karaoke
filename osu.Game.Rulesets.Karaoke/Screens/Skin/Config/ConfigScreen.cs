// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Config
{
    public class ConfigScreen : KaraokeSkinEditorScreen
    {
        [Cached]
        protected readonly LyricConfigManager ConfigManager;

        public ConfigScreen(ISkin skin)
            : base(skin, KaraokeSkinEditorScreenMode.Config)
        {
            AddInternal(ConfigManager = new LyricConfigManager());
        }

        protected override Section[] CreateSelectionContainer()
        {
            return new Section[] { };
        }

        protected override Section[] CreatePropertiesContainer()
        {
            return new Section[]
            {
                new IntervalSection(),
                new PositionSection(),
                new RubyRomajiSection()
            };
        }

        protected override Container CreatePreviewArea()
        {
            return new();
        }
    }
}
