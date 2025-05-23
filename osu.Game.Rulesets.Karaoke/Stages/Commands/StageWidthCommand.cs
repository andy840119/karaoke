// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;

namespace osu.Game.Rulesets.Karaoke.Stages.Commands;

public class StageWidthCommand : StageCommand<float>
{
    public StageWidthCommand(Easing easing, double startTime, double endTime, float startValue, float endValue)
        : base(easing, startTime, endTime, startValue, endValue)
    {
    }

    public override string PropertyName => nameof(Drawable.Width);

    public override void ApplyInitialValue<TDrawable>(TDrawable d) => d.Width = StartValue;

    public override TransformSequence<TDrawable> ApplyTransforms<TDrawable>(TDrawable d)
        => d.ResizeWidthTo(StartValue).Then().ResizeWidthTo(EndValue, Duration, Easing);
}
