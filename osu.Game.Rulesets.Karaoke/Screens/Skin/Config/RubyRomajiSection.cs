﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Config
{
    internal class RubyRomajiSection : LyricConfigSection
    {
        private LabelledEnumDropdown<LyricTextAlignment> rubyAlignmentDropdown;
        private LabelledEnumDropdown<LyricTextAlignment> romajiAlignmentDropdown;
        private LabelledRealTimeSliderBar<int> rubyMarginSliderBar;
        private LabelledRealTimeSliderBar<int> romajiMarginSliderBar;

        protected override LocalisableString Title => "Ruby/Romaji";

        [BackgroundDependencyLoader]
        private void load(LyricConfigManager manager)
        {
            Children = new Drawable[]
            {
                rubyAlignmentDropdown = new LabelledEnumDropdown<LyricTextAlignment>
                {
                    Label = "Ruby alignment",
                    Description = "Ruby alignment section",
                },
                romajiAlignmentDropdown = new LabelledEnumDropdown<LyricTextAlignment>
                {
                    Label = "Romaji alignment",
                    Description = "Romaji alignment section",
                },
                rubyMarginSliderBar = new LabelledRealTimeSliderBar<int>
                {
                    Label = "Ruby margin",
                    Description = "Ruby margin section",
                    Current = new BindableNumber<int>
                    {
                        MinValue = 0,
                        MaxValue = 30,
                        Value = 10,
                        Default = 10
                    }
                },
                romajiMarginSliderBar = new LabelledRealTimeSliderBar<int>
                {
                    Label = "Romaji margin",
                    Description = "Romaji margin section",
                    Current = new BindableNumber<int>
                    {
                        MinValue = 0,
                        MaxValue = 30,
                        Value = 10,
                        Default = 10
                    }
                }
            };

            manager.LoadedLyricFontInfo.BindValueChanged(e =>
            {
                var lyricFontInfo = e.NewValue;
                applyCurrent(rubyAlignmentDropdown.Current, lyricFontInfo.RubyAlignment);
                applyCurrent(romajiAlignmentDropdown.Current, lyricFontInfo.RomajiAlignment);
                applyCurrent(rubyMarginSliderBar.Current, lyricFontInfo.RubyMargin);
                applyCurrent(romajiMarginSliderBar.Current, lyricFontInfo.RomajiMargin);

                static void applyCurrent<T>(Bindable<T> bindable, T value)
                    => bindable.Value = bindable.Default = value;
            }, true);

            rubyAlignmentDropdown.Current.BindValueChanged(x => manager.ApplyCurrentLyricConfigChange(l => l.RubyAlignment = x.NewValue));
            romajiAlignmentDropdown.Current.BindValueChanged(x => manager.ApplyCurrentLyricConfigChange(l => l.RomajiAlignment = x.NewValue));
            rubyMarginSliderBar.Current.BindValueChanged(x => manager.ApplyCurrentLyricConfigChange(l => l.RubyMargin = x.NewValue));
            romajiMarginSliderBar.Current.BindValueChanged(x => manager.ApplyCurrentLyricConfigChange(l => l.RomajiMargin = x.NewValue));
        }
    }
}
