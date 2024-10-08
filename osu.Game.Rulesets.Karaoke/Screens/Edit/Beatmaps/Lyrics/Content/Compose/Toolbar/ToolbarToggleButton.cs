﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose.Toolbar;

/// <summary>
/// Button for toggle open and close.
/// </summary>
public abstract partial class ToolbarToggleButton : ToolbarButton
{
    protected readonly Bindable<bool> Active = new();

    protected ToolbarToggleButton()
    {
        Active.BindValueChanged(x =>
        {
            // should wait until set icon done.
            Schedule(() =>
            {
                toggle(x.NewValue);
            });
        }, true);

        Action = () =>
        {
            Active.Value = !Active.Value;
        };
    }

    private void toggle(bool active)
    {
        IconContainer.Icon.Alpha = active ? 1 : 0.5f;
    }
}
