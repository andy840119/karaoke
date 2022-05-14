// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public struct Tone : IComparable<Tone>, IComparable<int>, IEquatable<Tone>, IEquatable<int>
    {
        public int Scale { get; set; }

        public bool Half { get; set; }

        public Tone(int scale, bool half = false)
        {
            Scale = scale;
            Half = half;
        }

        public int CompareTo(Tone other)
        {
            if (Scale > other.Scale)
                return 1;

            if (Scale < other.Scale)
                return -1;

            if (Half == other.Half)
                return 0;

            if (Half)
                return 1;

            return -1;
        }

        public int CompareTo(int other)
        {
            if (Scale > other)
                return 1;

            if (Scale < other)
                return -1;

            return Half ? 1 : 0;
        }

        public bool Equals(Tone other)
        {
            return Scale == other.Scale && Half == other.Half;
        }

        public bool Equals(int other)
        {
            return Scale == other && Half == false;
        }

        public override bool Equals(object obj)
        {
            return obj switch
            {
                Tone tone => Equals(tone),
                int intValue => Equals(intValue),
                _ => false
            };
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Tone operator +(Tone left, Tone right)
        {
            return add(left, right);
        }

        public static Tone operator +(Tone tone1, int scale)
        {
            return tone1 + new Tone { Scale = scale };
        }

        private static Tone add(Tone tone1, Tone tone2)
        {
            return new()
            {
                Scale = tone1.Scale + tone2.Scale + (tone1.Half && tone2.Half ? 1 : 0),
                Half = tone1.Half ^ tone2.Half
            };
        }

        public static Tone operator -(Tone tone1, Tone tone2)
        {
            return subtract(tone1, tone2);
        }

        public static Tone operator -(Tone tone1, int scale)
        {
            return tone1 - new Tone { Scale = scale };
        }

        private static Tone subtract(Tone tone1, Tone tone2)
        {
            return tone1 + -tone2;
        }

        public static Tone operator -(Tone tone)
        {
            return negate(tone);
        }

        private static Tone negate(Tone tone)
        {
            return new()
            {
                Scale = -tone.Scale + (tone.Half ? -1 : 0),
                Half = tone.Half
            };
        }

        public static bool operator ==(Tone tone1, Tone tone2)
        {
            return tone1.Equals(tone2);
        }

        public static bool operator !=(Tone tone1, Tone tone2)
        {
            return !tone1.Equals(tone2);
        }

        public static bool operator ==(Tone tone1, int tone2)
        {
            return tone1.Equals(tone2);
        }

        public static bool operator !=(Tone tone1, int tone2)
        {
            return !tone1.Equals(tone2);
        }

        public static bool operator >(Tone tone1, Tone tone2)
        {
            return tone1.CompareTo(tone2) > 0;
        }

        public static bool operator >=(Tone tone1, Tone tone2)
        {
            return tone1.CompareTo(tone2) >= 0;
        }

        public static bool operator <(Tone tone1, Tone tone2)
        {
            return tone1.CompareTo(tone2) < 0;
        }

        public static bool operator <=(Tone tone1, Tone tone2)
        {
            return tone1.CompareTo(tone2) <= 0;
        }

        public static bool operator >(Tone tone1, int tone2)
        {
            return tone1.CompareTo(tone2) > 0;
        }

        public static bool operator >=(Tone tone1, int tone2)
        {
            return tone1.CompareTo(tone2) >= 0;
        }

        public static bool operator <(Tone tone1, int tone2)
        {
            return tone1.CompareTo(tone2) < 0;
        }

        public static bool operator <=(Tone tone1, int tone2)
        {
            return tone1.CompareTo(tone2) <= 0;
        }

        public override string ToString()
        {
            return $"Scale:{Scale}, Half:{Half}";
        }
    }
}
