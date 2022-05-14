// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Graphics;
using osu.Game.Replays;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.UI.Position;

namespace osu.Game.Rulesets.Karaoke.UI.Components
{
    public class ReplaySaitenVisualization : VoiceVisualization<KaraokeReplayFrame>
    {
        protected override float PathRadius => 1.5f;

        private bool createNew = true;

        private double minAvailableTime;

        [Resolved]
        private INotePositionInfo notePositionInfo { get; set; }

        public ReplaySaitenVisualization(Replay replay)
        {
            var frames = replay?.Frames.OfType<KaraokeReplayFrame>();
            frames?.ForEach(Add);
        }

        public void Add(KaraokeReplayFrame frame)
        {
            // Start time should be largest and cannot be removed.
            double startTime = frame.Time;
            if (startTime <= minAvailableTime)
                throw new ArgumentOutOfRangeException(nameof(startTime));

            minAvailableTime = startTime;

            if (!frame.Sound)
            {
                // Next replay frame will create new path
                createNew = true;
                return;
            }

            if (createNew)
            {
                createNew = false;

                CreateNew(frame);
            }
            else
                Append(frame);
        }

        protected override double GetTime(KaraokeReplayFrame frame)
        {
            return frame.Time;
        }

        protected override float GetPosition(KaraokeReplayFrame frame)
        {
            return notePositionInfo.Calculator.YPositionAt(frame);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Colour = colours.GrayF;
        }
    }
}
