// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Saiten;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    // todo: check what the class is this.
    public class NoteGenerateSaitenPathSection : Section
    {
        protected override string Title => "Generate saiten path";

        private Bindable<float> bindableProgress;
        private Bindable<IDictionary<double, float?>> bindableVoiceData;

        private ExecuteButton button;
        private OsuSpriteText result;

        [BackgroundDependencyLoader]
        private void load(SaitenManager saitenManager)
        {
            Children = new Drawable[]
            {
                button = new ExecuteButton
                {
                    Text = "Execute",
                    Action = () =>
                    {
                        saitenManager?.ConvertSingerVoiceToData();
                    }
                },
                result = new OsuSpriteText(),
                new LabelledRealTimeSliderBar<float>
                {
                    Label = "Scale",
                    Current = saitenManager.PitchToScaleScale,
                },
                new LabelledRealTimeSliderBar<float>
                {
                    Label = "Panning",
                    Current = saitenManager.PitchToScalePanning,
                }
            };

            bindableProgress = saitenManager.BindableProgress.GetBoundCopy();
            bindableVoiceData = saitenManager.BindableVoiceData.GetBoundCopy();
            bindableProgress.BindValueChanged(e =>
            {
                // todo : prevent running on other thread.
                Schedule(() =>
                {
                    var precentage = e.NewValue * 100;
                    button.Text = $"...{precentage:N2}%";
                });
            });
            bindableVoiceData.BindValueChanged((e) =>
            {
                button.Text = "Execute";
                result.Text = "Complete!";
            });
        }

        public class ExecuteButton : OsuButton
        {
            public ExecuteButton()
            {
                RelativeSizeAxes = Axes.X;
                Content.CornerRadius = 15;
            }
        }
    }
}
