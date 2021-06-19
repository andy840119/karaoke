// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Saiten;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class EditorNotePlayfield : ScrollingNotePlayfield
    {
        public EditorNotePlayfield(int columns)
            : base(columns)
        {
            BackgroundLayer.AddRange(new Drawable[]
            {
                new Box
                {
                    Depth = 1,
                    Name = "Background",
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.5f
                },
            });

            HitObjectArea.Add(new SingerVoiceVisualization
            {
                Name = "Saiten Visualization",
                RelativeSizeAxes = Axes.Both,
                Alpha = 0.6f
            });
        }

        public class SingerVoiceVisualization : VoiceVisualization<KeyValuePair<double, float?>>
        {
            private Bindable<IDictionary<double, float?>> bindableVoiceData;
            private Bindable<float> pitchToScaleScale;
            private Bindable<float> pitchToScalePanning;

            protected override double GetTime(KeyValuePair<double, float?> point) => point.Key;

            protected override float GetPosition(KeyValuePair<double, float?> point)
            {
                if (!point.Value.HasValue)
                    return 0;

                var scale = pitchToScaleScale.Value;
                var panning = pitchToScalePanning.Value;
                return point.Value.Value * scale + panning;
            }

            private bool createNew = true;

            private double minAvailableTime;

            public void Add(KeyValuePair<double, float?> point)
            {
                // Start time should be largest and cannot be removed.
                var startTime = point.Key;
                if (startTime <= minAvailableTime)
                    return;

                minAvailableTime = startTime;

                if (!point.Value.HasValue)
                {
                    // Next replay frame will create new path
                    createNew = true;
                    return;
                }

                if (createNew)
                {
                    createNew = false;

                    CreateNew(point);
                }
                else
                {
                    Append(point);
                }
            }

            [BackgroundDependencyLoader(true)]
            private void load(OsuColour colours, SaitenManager saitenManager)
            {
                Colour = colours.GrayF;

                bindableVoiceData = saitenManager.BindableVoiceData.GetBoundCopy();
                pitchToScaleScale = saitenManager.PitchToScaleScale.GetBoundCopy();
                pitchToScalePanning = saitenManager.PitchToScalePanning.GetBoundCopy();

                bindableVoiceData.BindValueChanged(e =>
                {
                    // todo : prevent running on other thread.
                    Schedule(() =>
                    {
                        Clear();
                        minAvailableTime = 0;
                        e.NewValue?.ForEach(Add);
                    });
                }, true);
                pitchToScaleScale.BindValueChanged(e =>
                {
                    Invalid();
                }, true);
                pitchToScalePanning.BindValueChanged(e =>
                {
                    Invalid();
                }, true);
            }
        }
    }
}
