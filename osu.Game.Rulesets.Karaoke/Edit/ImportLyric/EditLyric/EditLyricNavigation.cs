// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.EditLyric
{
    public class EditLyricNavigation : TopNavigation<EditLyricStepScreen>
    {
        private const string cutting_mode = "CUTTING_MODE";
        private const string typing_mode = "TYPING_MODE";

        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();

        public EditLyricNavigation(EditLyricStepScreen screen)
            : base(screen)
        {
            bindableMode.BindValueChanged(_ =>
            {
                // should update the display text in navigation bar if mode change.
                TriggerStateChange();
            });
        }

        protected override NavigationTextContainer CreateTextContainer()
        {
            return new EditLyricTextFlowContainer(Screen);
        }

        protected override NavigationState GetState(Lyric[] lyrics)
        {
            return NavigationState.Working;
        }

        protected override string GetNavigationText(NavigationState value)
        {
            switch (value)
            {
                case NavigationState.Initial:
                    return $"Does something looks weird? Try switching [{cutting_mode}] or [{typing_mode}] to edit your lyric.";

                case NavigationState.Working:
                case NavigationState.Done:
                    var mode = Screen.LyricEditorMode;

                    return mode switch
                    {
                        LyricEditorMode.Manage => $"Cool! Try switching to [{typing_mode}] if you want to edit lyric.",
                        LyricEditorMode.Typing => $"Cool! Try switching to [{cutting_mode}] if you want to cut or combine lyric.",
                        _ => throw new InvalidEnumArgumentException(nameof(mode))
                    };

                case NavigationState.Error:
                    return "Oops, seems cause some error in here.";

                default:
                    throw new InvalidEnumArgumentException(nameof(value));
            }
        }

        protected override bool AbleToNextStep(NavigationState value)
        {
            return true;
        }

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state)
        {
            bindableMode.BindTo(state.BindableMode);
        }

        private class EditLyricTextFlowContainer : NavigationTextContainer
        {
            public EditLyricTextFlowContainer(EditLyricStepScreen screen)
            {
                AddLinkFactory(cutting_mode, "cutting mode", () => screen.SwitchLyricEditorMode(LyricEditorMode.Manage));
                AddLinkFactory(typing_mode, "typing mode", () => screen.SwitchLyricEditorMode(LyricEditorMode.Typing));
            }
        }
    }
}
