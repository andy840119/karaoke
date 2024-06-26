// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage.Settings;

public partial class StageEditorSettingsHeader : EditorSettingsHeader<StageEditorEditMode>
{
    private readonly StageEditorEditCategory category;

    public StageEditorSettingsHeader(StageEditorEditCategory category)
    {
        this.category = category;
    }

    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Green;

    protected override EditStepTabControl CreateTabControl()
        => new StageEditStepTabControl(category);

    protected override DescriptionFormat GetSelectionDescription(StageEditorEditMode step) =>
        step switch
        {
            StageEditorEditMode.Edit => "Edit the stage property in here.",
            StageEditorEditMode.Verify => "Check if have any stage issues.",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    private partial class StageEditStepTabControl : EditStepTabControl
    {
        private readonly StageEditorEditCategory category;

        public StageEditStepTabControl(StageEditorEditCategory category)
        {
            this.category = category;
        }

        protected override StepTabButton CreateStepButton(OsuColour colours, StageEditorEditMode value)
        {
            return value switch
            {
                StageEditorEditMode.Edit => new StepTabButton(value)
                {
                    Text = "Edit",
                    SelectedColour = colours.Red,
                    UnSelectedColour = colours.RedDarker,
                },
                StageEditorEditMode.Verify => new VerifyStepTabButton(value, category)
                {
                    Text = "Verify",
                    SelectedColour = colours.Yellow,
                    UnSelectedColour = colours.YellowDarker,
                },
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
        }
    }

    private partial class VerifyStepTabButton : IssueStepTabButton
    {
        private readonly StageEditorEditCategory category;

        public VerifyStepTabButton(StageEditorEditMode value, StageEditorEditCategory category)
            : base(value)
        {
            this.category = category;
        }

        [BackgroundDependencyLoader]
        private void load(IStageEditorVerifier stageEditorVerifier)
        {
            Issues.BindTo(stageEditorVerifier.GetIssueByType(category));
        }
    }
}
