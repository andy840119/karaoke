﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Screens.Play.PlayerSettings;

namespace osu.Game.Rulesets.Karaoke.UI.HUD
{
    public class SettingOverlayContainer : CompositeDrawable, IKeyBindingHandler<KaraokeAction>, ISettingHUDOverlay
    {
        public Action<SettingOverlay> OnNewOverlayAdded;
        private GeneralSettingOverlay generalSettingsOverlay;

        public void ToggleGeneralSettingsOverlay()
        {
            generalSettingsOverlay.ToggleVisibility();
        }

        public virtual bool OnPressed(KeyBindingPressEvent<KaraokeAction> e)
        {
            switch (e.Action)
            {
                // Open adjustment overlay
                case KaraokeAction.OpenPanel:
                    ToggleGeneralSettingsOverlay();
                    return true;

                default:
                    return false;
            }
        }

        public virtual void OnReleased(KeyBindingReleaseEvent<KaraokeAction> e)
        {
        }

        public void AddSettingsGroup(PlayerSettingsGroup group)
        {
            generalSettingsOverlay.Add(group);
        }

        public void AddExtraOverlay(SettingOverlay overlay)
        {
            AddInternal(overlay);
            OnNewOverlayAdded?.Invoke(overlay);
        }

        public void ChangeOverlayDirection(OverlayDirection direction)
        {
            foreach (var settingOverlay in InternalChildren.OfType<SettingOverlay>()) settingOverlay.Direction = direction;
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            var dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

            // use tricky way to get session from karaoke ruleset.
            object drawableRuleset = dependencies.Get(typeof(DrawableRuleset));

            if (drawableRuleset is not DrawableKaraokeRuleset drawableKaraokeRuleset)
                return dependencies;

            dependencies.CacheAs(drawableKaraokeRuleset.Config);
            dependencies.CacheAs(drawableKaraokeRuleset.Session);

            return dependencies;
        }

        [BackgroundDependencyLoader]
        private void load(IBindable<IReadOnlyList<Mod>> mods)
        {
            AddExtraOverlay(generalSettingsOverlay = new GeneralSettingOverlay());

            if (mods == null)
                return;

            foreach (var mod in mods.Value.OfType<IApplicableToSettingHUDOverlay>())
                mod.ApplyToOverlay(this);
        }
    }
}
