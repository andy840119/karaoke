﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Screens.Play.PlayerSettings;

namespace osu.Game.Rulesets.Karaoke.UI.PlayerSettings
{
    public class TranslateSettings : PlayerSettingsGroup
    {
        private readonly PlayerCheckbox translateCheckBox;
        private readonly OsuSpriteText translateText;
        private readonly OsuDropdown<CultureInfo> translateDropDown;

        public TranslateSettings()
            : base("Translate")
        {
            Children = new Drawable[]
            {
                translateCheckBox = new PlayerCheckbox
                {
                    LabelText = "Translate"
                },
                translateText = new OsuSpriteText
                {
                    Text = "Translate language"
                },
                translateDropDown = new OsuDropdown<CultureInfo>
                {
                    RelativeSizeAxes = Axes.X
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(IBindable<WorkingBeatmap> beatmap, KaraokeSessionStatics session)
        {
            translateDropDown.Items = beatmap.Value.Beatmap.AvailableTranslates();

            // Translate
            translateCheckBox.Current = session.GetBindable<bool>(KaraokeRulesetSession.UseTranslate);
            translateDropDown.Current = session.GetBindable<CultureInfo>(KaraokeRulesetSession.PreferLanguage);

            // hidden dropdown if not translate
            translateCheckBox.Current.BindValueChanged(value =>
            {
                if (value.NewValue)
                {
                    translateText.Show();
                    translateDropDown.Show();
                }
                else
                {
                    translateText.Hide();
                    translateDropDown.Hide();
                }
            }, true);
        }
    }
}
