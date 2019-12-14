﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.UI.Scrolling;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables
{
    public abstract class DrawableKaraokeScrollingHitObject : DrawableHitObject<KaraokeHitObject>
    {
        /// <summary>
        /// Whether this <see cref="DrawableKaraokeHitObject"/> should always remain alive.
        /// </summary>
        internal bool AlwaysAlive;

        protected readonly IBindable<ScrollingDirection> Direction = new Bindable<ScrollingDirection>();

        protected readonly IBindable<double> TimeRange = new Bindable<double>();

        [System.Obsolete]
        protected override bool UseTransformStateManagement => false;

        protected DrawableKaraokeScrollingHitObject(KaraokeHitObject hitObject)
            : base(hitObject)
        {
        }

        [BackgroundDependencyLoader(true)]
        private void load([NotNull] IScrollingInfo scrollingInfo)
        {
            Direction.BindTo(scrollingInfo.Direction);
            Direction.BindValueChanged(OnDirectionChanged, true);

            TimeRange.BindTo(scrollingInfo.TimeRange);
            TimeRange.BindValueChanged(OnTimeRangeChanged, true);
        }

        protected override bool ShouldBeAlive => AlwaysAlive || base.ShouldBeAlive;

        protected virtual void OnDirectionChanged(ValueChangedEvent<ScrollingDirection> e)
        {
            Anchor = Origin = e.NewValue == ScrollingDirection.Left ? Anchor.CentreLeft : Anchor.CentreRight;
        }

        protected virtual void OnTimeRangeChanged(ValueChangedEvent<double> e)
        {
            // Adjust life time
            LifetimeEnd = HitObject.GetEndTime() + e.NewValue;
        }
    }

    public abstract class DrawableKaraokeScrollingHitObject<TObject> : DrawableKaraokeScrollingHitObject
        where TObject : KaraokeHitObject
    {
        public new readonly TObject HitObject;

        protected DrawableKaraokeScrollingHitObject(TObject hitObject)
            : base(hitObject)
        {
            HitObject = hitObject;
        }
    }
}
