﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.UI.HUD;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.UI
{
    [TestFixture]
    public class TestSceneControlLayer : OsuTestScene
    {
        public SettingOverlayContainer SettingOverlayContainer { get; set; }

        protected override Ruleset CreateRuleset()
        {
            return new KaraokeRuleset();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            var config = Dependencies.Get<KaraokeRulesetConfigManager>();
            Dependencies.Cache(new KaraokeSessionStatics(config, null));

            // Cannot work now because it need extra BDL in child
            Add(new Container
            {
                RelativeSizeAxes = Axes.Both,
                Child = SettingOverlayContainer = new SettingOverlayContainer
                {
                    RelativeSizeAxes = Axes.Both
                }
            });

            AddStep("Toggle setting", SettingOverlayContainer.ToggleGeneralSettingsOverlay);
        }
    }
}
