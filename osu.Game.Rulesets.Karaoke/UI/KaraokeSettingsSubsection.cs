﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class KaraokeSettingsSubsection : RulesetSettingsSubsection
    {
        protected override string Header => "osu!karaoke";

        public KaraokeSettingsSubsection(Ruleset ruleset)
            : base(ruleset)
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            var config = (KaraokeRulesetConfigManager)Config;

            Children = new Drawable[]
            {
                // Visual
                new SettingsEnumDropdown<KaraokeScrollingDirection>
                {
                    LabelText = "Scrolling direction",
                    Bindable = config.GetBindable<KaraokeScrollingDirection>(KaraokeRulesetSetting.ScrollDirection)
                },
                new SettingsSlider<double, TimeSlider>
                {
                    LabelText = "Scroll speed",
                    Bindable = config.GetBindable<double>(KaraokeRulesetSetting.ScrollTime)
                },
                new SettingsCheckbox
                {
                    LabelText = "Display alternative text",
                    Bindable = config.GetBindable<bool>(KaraokeRulesetSetting.DisplayAlternativeText)
                },
                new SettingsCheckbox
                {
                    LabelText = "Show cursor while playing",
                    Bindable = config.GetBindable<bool>(KaraokeRulesetSetting.ShowCursor)
                },
                // Translate
                new SettingsCheckbox
                {
                    LabelText = "Translate",
                    Bindable = config.GetBindable<bool>(KaraokeRulesetSetting.UseTranslate)
                },
                new SettingsTextBox
                {
                    LabelText = "Prefer language",
                    Bindable = config.GetBindable<string>(KaraokeRulesetSetting.PreferLanguage)
                },
                // Pitch
                new SettingsCheckbox
                {
                    LabelText = "Override pitch at gameplay",
                    Bindable = config.GetBindable<bool>(KaraokeRulesetSetting.OverridePitchAtGameplay)
                },
                new SettingsSlider<int, PitchSlider>
                {
                    LabelText = "Pitch",
                    Bindable = config.GetBindable<int>(KaraokeRulesetSetting.Pitch)
                },
                new SettingsCheckbox
                {
                    LabelText = "Override vocal pitch at gameplay",
                    Bindable = config.GetBindable<bool>(KaraokeRulesetSetting.OverrideVocalPitchAtGameplay)
                },
                new SettingsSlider<int, PitchSlider>
                {
                    LabelText = "Vocal pitch",
                    Bindable = config.GetBindable<int>(KaraokeRulesetSetting.VocalPitch)
                },
                new SettingsCheckbox
                {
                    LabelText = "Override saiten pitch at gameplay",
                    Bindable = config.GetBindable<bool>(KaraokeRulesetSetting.OverrideSaitenPitchAtGameplay)
                },
                new SettingsSlider<int, PitchSlider>
                {
                    LabelText = "Saiten pitch",
                    Bindable = config.GetBindable<int>(KaraokeRulesetSetting.SaitenPitch)
                },
                // Practice
                new SettingsSlider<double, TimeSlider>
                {
                    LabelText = "Practice preempt time",
                    Bindable = config.GetBindable<double>(KaraokeRulesetSetting.PracticePreemptTime)
                },
            };
        }

        private class PitchSlider : OsuSliderBar<int>
        {
            public override string TooltipText => (Current.Value >= 0 ? "+" : "") + Current.Value.ToString("N0");
        }

        private class TimeSlider : OsuSliderBar<double>
        {
            public override string TooltipText => Current.Value.ToString("N0") + "ms";
        }
    }
}