﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Overlays;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages.Settings;

public partial class PageSettings : EditorSettings
{
    private readonly Bindable<PageEditorEditMode> bindableMode = new();

    [BackgroundDependencyLoader]
    private void load(OverlayColourProvider colourProvider, IPageStateProvider pageStateProvider)
    {
        bindableMode.BindTo(pageStateProvider.BindableEditMode);

        // change the background colour to the lighter one.
        ChangeBackgroundColour(colourProvider.Background3);
    }

    protected override EditorSettingsHeader CreateSettingHeader()
        => new PageEditorSettingsHeader
        {
            Current = bindableMode,
        };

    protected override IReadOnlyList<EditorSection> CreateEditorSections() => bindableMode.Value switch
    {
        PageEditorEditMode.Generate => new[]
        {
            new PageAutoGenerateSection(),
        },
        PageEditorEditMode.Edit => new[]
        {
            new PagesSection(),
        },
        PageEditorEditMode.Verify => new[]
        {
            new PageEditorIssueSection(),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };
}
