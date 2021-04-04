﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Screens.Config.Sections;

namespace osu.Game.Rulesets.Karaoke.Screens.Config
{
    public class KaraokeSettingsPanel : SettingsPanel
    {
        protected override IEnumerable<SettingsSection> CreateSections() => new SettingsSection[]
        {
            new ConfigSection(),
            new StyleSection(),
            new ScoringSection()
        };

        protected override SettingsSectionsContainer CreateSettingsSections() => new KaraokeSettingsSectionsContainer();

        protected override Drawable CreateFooter() => new Container
        {
            Height = 130,
        };

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            var dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

            var config = dependencies.Get<RulesetConfigCache>().GetConfigFor(new KaraokeRuleset());
            if (config != null)
                dependencies.Cache(config);

            return dependencies;
        }

        [BackgroundDependencyLoader]
        private void load(ConfigColourProvider colourProvider, Bindable<SettingsSection> selectedSection)
        {
            selectedSection.BindValueChanged(x =>
            {
                var colour = colourProvider.GetBackground3Colour(x.NewValue);
                Background.Delay(200).Then().FadeColour(colour, 500);
            });
        }

        public class KaraokeSettingsSectionsContainer : SettingsSectionsContainer
        {
            private UserTrackingScrollContainer scrollContainer;
            private Box background;

            protected override UserTrackingScrollContainer CreateScrollContainer()
                => scrollContainer = base.CreateScrollContainer();

            [BackgroundDependencyLoader]
            private void load(ConfigColourProvider colourProvider, Bindable<SettingsSection> selectedSection, Bindable<SettingsSubsection> selectedSubsection)
            {
                // create hove background.
                scrollContainer.Add(background = new Box
                {
                    RelativeSizeAxes = Axes.X,
                    Colour = Colour4.Red,
                    Depth = 1,
                    Alpha = 0.3f
                });

                selectedSection.BindValueChanged(x =>
                {
                    var colour = colourProvider.GetBackgroundColour(x.NewValue);
                    background.Delay(200).Then().FadeColour(colour, 500);
                });

                selectedSubsection.BindValueChanged(x =>
                {
                    var offset = 20;
                    var position = scrollContainer.GetChildPosInContent(x.NewValue);
                    background.Y = position + offset;
                    background.Height = x.NewValue.DrawHeight;
                });
            }
        }
    }
}
