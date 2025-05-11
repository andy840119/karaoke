// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Config;

public partial class LyricFontInfoManager : Component
{
    public readonly BindableList<LyricFontInfo> Configs = new();

    public readonly Bindable<LyricFontInfo> LoadedLyricFontInfo = new();

    public readonly Bindable<LyricFontInfo> EditLyricFontInfo = new();

    // todo: should get the skin from the stage info.
    [BackgroundDependencyLoader]
    private void load()
    {
    }

    public void ApplyCurrentLyricFontInfoChange(Action<LyricFontInfo> action)
    {
        action(LoadedLyricFontInfo.Value);
        LoadedLyricFontInfo.TriggerChange();
    }
}
