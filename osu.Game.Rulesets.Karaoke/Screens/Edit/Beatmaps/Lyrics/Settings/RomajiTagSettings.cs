﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Romanisation;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public partial class RomajiTagSettings : TextTagSettings<RomanisationTagEditStep>
{
    [BackgroundDependencyLoader]
    private void load(IEditRomanisationModeState romanisationModeState)
    {
        BindableEditStep.BindTo(romanisationModeState.BindableEditStep);
        BindableEditStep.BindValueChanged(e =>
        {
            ReloadSections();
        }, true);
    }

    protected override IReadOnlyList<Drawable> CreateSections() => BindableEditStep.Value switch
    {
        RomanisationTagEditStep.Generate => new Drawable[]
        {
            new RomanisationEditStepSection(),
            new RomanisationAutoGenerateSection(),
        },
        RomanisationTagEditStep.Edit => new Drawable[]
        {
            new RomanisationEditStepSection(),
            new RomanisationEditPropertyModeSection(),
            new RomanisationEditSection(),
        },
        RomanisationTagEditStep.Verify => new Drawable[]
        {
            new RomanisationEditStepSection(),
            new RomanisationIssueSection(),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };
}
