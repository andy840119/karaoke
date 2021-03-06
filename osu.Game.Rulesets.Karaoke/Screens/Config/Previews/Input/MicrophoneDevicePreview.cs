﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Input;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Previews.Input
{
    public class MicrophoneDevicePreview : SettingsSubsectionPreview
    {
        private readonly Bindable<string> bindableMicrophoneDeviceName = new Bindable<string>();

        public MicrophoneDevicePreview()
        {
            ShowBackground = false;
            bindableMicrophoneDeviceName.BindValueChanged(x =>
            {
                // Find index by selection id
                var microphoneList = new MicrophoneManager().MicrophoneDeviceNames.ToList();
                var deviceName = x.NewValue;
                var deviceIndex = microphoneList.IndexOf(x.NewValue);
                var hasDevice = !string.IsNullOrEmpty(x.NewValue);

                Child = new MicrophoneInputManager(deviceIndex)
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = new MicrophoneSoundVisualizer
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        DeviceName = deviceName,
                        HasDevice = hasDevice,
                    }
                };
            }, true);
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetConfigManager config)
        {
            config.BindWith(KaraokeRulesetSetting.MicrophoneDevice, bindableMicrophoneDeviceName);
        }
    }
}
