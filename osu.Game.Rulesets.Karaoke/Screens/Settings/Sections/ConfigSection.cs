﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Screens.Settings.Sections.Gameplay;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings.Sections
{
    public class ConfigSection : KaraokeSettingsSection
    {
        public override LocalisableString Header => "Config";

        public ConfigSection()
        {
            Children = new Drawable[]
            {
                new GeneralSettings(),
                new NoteSettings(),
                new LyricSettings(),
                new TranslateSettings()
            };
        }

        public override Drawable CreateIcon()
        {
            return new SpriteIcon
            {
                Icon = FontAwesome.Solid.Cog
            };
        }
    }
}
