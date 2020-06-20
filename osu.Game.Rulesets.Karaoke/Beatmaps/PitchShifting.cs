// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Beatmaps
{
    public struct PitchShifting
    {
        public float Shifting { get; set; }

        public float Scale { get; set; }

        public float ScaleShifting(float scale)
        {
            return scale * Scale + Shifting;
        }
    }
}
