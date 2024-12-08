// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Shared;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Stages.Commands.Lyrics;

public class LyricFontInfoCommand : StageCommand<LyricFontInfo>
{
    public LyricFontInfoCommand(Easing easing, double startTime, double endTime, LyricFontInfo startValue, LyricFontInfo endValue)
        : base(easing, startTime, endTime, startValue, endValue)
    {
    }

    public override string PropertyName => "LyricFontInfo";

    public override void ApplyInitialValue<TDrawable>(TDrawable d)
    {
        if (d is not DrawableLyric drawableLyric)
            throw new InvalidOperationException();

        drawableLyric.ApplyToLyricPieces(l =>
        {
            // Apply text font info
            l.Font = StartValue.MainTextFont;
            l.TopTextFont = StartValue.RubyTextFont;
            l.BottomTextFont = StartValue.RomanisationTextFont;

            // Layout to text
            l.KaraokeTextSmartHorizon = StartValue.SmartHorizon;
            l.Spacing = new Vector2(StartValue.LyricsInterval, l.Spacing.Y);

            // Top text
            l.TopTextSpacing = new Vector2(StartValue.RubyInterval, l.TopTextSpacing.Y);
            l.TopTextAlignment = StartValue.RubyAlignment;
            l.TopTextMargin = StartValue.RubyMargin;

            // Bottom text
            l.BottomTextSpacing = new Vector2(StartValue.RomanisationInterval, l.BottomTextSpacing.Y);
            l.BottomTextAlignment = StartValue.RomanisationAlignment;
            l.BottomTextMargin = StartValue.RomanisationMargin;
        });
    }

    public override TransformSequence<TDrawable> ApplyTransforms<TDrawable>(TDrawable d)
    {
        return d.TransformTo(new LyricFontTransform(StartValue))
                .Delay(Duration)
                .Append(x =>
                {
                    var transform = new LyricFontTransform(EndValue);
                    x.TransformTo(transform);
                });
    }

    private class LyricFontTransform : Transform<LyricFontInfo, DrawableLyric>
    {
        private readonly LyricFontInfo fontInfo;

        public LyricFontTransform(LyricFontInfo newValue)
        {
            fontInfo = newValue;
            TargetMember = nameof(newValue);
        }

        public override string TargetMember { get; }

        protected override void Apply(DrawableLyric d, double time)
        {
            d.ApplyToLyricPieces(l =>
            {
                // Apply text font info
                l.Font = fontInfo.MainTextFont;
                l.TopTextFont = fontInfo.RubyTextFont;
                l.BottomTextFont = fontInfo.RomanisationTextFont;

                // Layout to text
                l.KaraokeTextSmartHorizon = fontInfo.SmartHorizon;
                l.Spacing = new Vector2(fontInfo.LyricsInterval, l.Spacing.Y);

                // Top text
                l.TopTextSpacing = new Vector2(fontInfo.RubyInterval, l.TopTextSpacing.Y);
                l.TopTextAlignment = fontInfo.RubyAlignment;
                l.TopTextMargin = fontInfo.RubyMargin;

                // Bottom text
                l.BottomTextSpacing = new Vector2(fontInfo.RomanisationInterval, l.BottomTextSpacing.Y);
                l.BottomTextAlignment = fontInfo.RomanisationAlignment;
                l.BottomTextMargin = fontInfo.RomanisationMargin;
            });
        }

        protected override void ReadIntoStartValue(DrawableLyric d)
        {
            StartValue = fontInfo;
        }
    }
}
