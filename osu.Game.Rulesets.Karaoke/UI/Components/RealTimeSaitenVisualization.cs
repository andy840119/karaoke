// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Caching;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.UI.Position;

namespace osu.Game.Rulesets.Karaoke.UI.Components
{
    public class RealTimeSaitenVisualization : VoiceVisualization<KeyValuePair<double, KaraokeSaitenAction>>
    {
        protected override float PathRadius => 2.5f;

        protected override float Offset => DrawSize.X;
        private readonly Cached addStateCache = new();

        private bool createNew = true;

        private double minAvailableTime;

        [Resolved]
        private INotePositionInfo notePositionInfo { get; set; }

        public RealTimeSaitenVisualization()
        {
            Masking = true;
        }

        public void AddAction(KaraokeSaitenAction action)
        {
            if (Time.Current <= minAvailableTime)
                return;

            minAvailableTime = Time.Current;

            if (createNew)
            {
                createNew = false;

                CreateNew(new KeyValuePair<double, KaraokeSaitenAction>(Time.Current, action));
            }
            else
                Append(new KeyValuePair<double, KaraokeSaitenAction>(Time.Current, action));

            // Trigger update last frame
            addStateCache.Invalidate();
        }

        public void Release()
        {
            if (Time.Current < minAvailableTime)
                return;

            minAvailableTime = Time.Current;

            createNew = true;
        }

        protected override double GetTime(KeyValuePair<double, KaraokeSaitenAction> frame)
        {
            return frame.Key;
        }

        protected override float GetPosition(KeyValuePair<double, KaraokeSaitenAction> frame)
        {
            return notePositionInfo.Calculator.YPositionAt(frame.Value);
        }

        protected override void Update()
        {
            // If addStateCache is invalid, means last path should be re-calculate
            if (!addStateCache.IsValid && Paths.Any())
            {
                var updatePath = Paths.LastOrDefault();
                MarkAsInvalid(updatePath);
                addStateCache.Validate();
            }

            base.Update();
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Colour = colours.Yellow;
        }
    }
}
