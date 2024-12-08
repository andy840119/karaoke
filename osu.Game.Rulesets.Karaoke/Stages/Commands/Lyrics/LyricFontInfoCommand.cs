// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Stages.Commands.Lyrics;

public class LyricFontInfoCommand : StageCommand<LyricFontInfo>
{
    public LyricFontInfoCommand(Easing easing, double startTime, double endTime, LyricFontInfo startValue, LyricFontInfo endValue)
        : base(easing, startTime, endTime, startValue, endValue)
    {
    }

    public override string PropertyName => nameof(LyricFontInfo);

    public override void ApplyInitialValue<TDrawable>(TDrawable d)
    {
        if (d is not DrawableLyric drawableLyric)
            throw new InvalidOperationException();

        drawableLyric.ApplyToLyricPieces(l =>
        {
            l.UpdateFont(StartValue);
        });
    }

    public override TransformSequence<TDrawable> ApplyTransforms<TDrawable>(TDrawable d)
    {
        // note: because update shader cost lots of effect, if the duration is 0, we just use the initial value.
        if (Duration == 0)
            return d.Delay(0);

        return d.TransformTo(d.PopulateTransform(new LyricFontTransform(), StartValue))
                .Delay(Duration)
                .Append(o => o.TransformTo(d.PopulateTransform(new LyricFontTransform(), EndValue)));
    }

    private class LyricFontTransform : Transform<LyricFontInfo, Drawable>
    {
        public override string TargetMember => nameof(LyricFontInfo);

        protected override void Apply(Drawable d, double time)
        {
            if (d is not DrawableLyric drawableLyric)
                throw new InvalidOperationException();

            drawableLyric.ApplyToLyricPieces(l =>
            {
                l.UpdateFont(EndValue);
            });
        }

        protected override void ReadIntoStartValue(Drawable d)
        {
            // there's no start value for it.
        }
    }
}
