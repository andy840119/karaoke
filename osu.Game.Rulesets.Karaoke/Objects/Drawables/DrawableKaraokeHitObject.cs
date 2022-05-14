// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Judgements;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables
{
    public class DrawableKaraokeHitObject : DrawableHitObject<KaraokeHitObject>
    {
        protected sealed override double InitialLifetimeOffset => HitObject.TimePreempt;

        protected DrawableKaraokeHitObject(KaraokeHitObject hitObject)
            : base(hitObject)
        {
        }

        protected override JudgementResult CreateResult(Judgement judgement)
        {
            return new KaraokeJudgementResult(HitObject, judgement);
        }
    }
}
