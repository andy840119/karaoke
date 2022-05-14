﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Screens.Settings.Sections.Gameplay;
using osu.Game.Rulesets.Karaoke.Screens.Settings.Sections.Input;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings.Sections
{
    public class ScoringSection : KaraokeSettingsSection
    {
        public override LocalisableString Header => "Scoring";

        public ScoringSection()
        {
            Children = new Drawable[]
            {
                new MicrophoneSettings(),
                new SaitenSettings()
            };
        }

        public override Drawable CreateIcon()
        {
            return new SpriteIcon
            {
                Icon = FontAwesome.Solid.Gamepad
            };
        }
    }
}
