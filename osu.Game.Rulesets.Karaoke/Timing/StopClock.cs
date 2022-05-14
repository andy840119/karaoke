﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Timing;

namespace osu.Game.Rulesets.Karaoke.Timing
{
    public class StopClock : IFrameBasedClock
    {
        public double ElapsedFrameTime => 0;

        public double FramesPerSecond => 0;

        public FrameTimeInfo TimeInfo => new() { Current = CurrentTime, Elapsed = ElapsedFrameTime };

        public double CurrentTime { get; }

        public double Rate => 0;

        public bool IsRunning => false;

        public StopClock(double targetTime)
        {
            CurrentTime = targetTime;
        }

        public void ProcessFrame()
        {
        }
    }
}
