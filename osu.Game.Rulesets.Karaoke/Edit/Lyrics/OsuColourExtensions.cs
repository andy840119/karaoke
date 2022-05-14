﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public static class OsuColourExtensions
    {
        public static Color4 GetTimeTagColour(this OsuColour colours, TimeTag timeTag)
        {
            bool hasTime = timeTag.Time.HasValue;
            if (!hasTime)
                return colours.Gray7;

            return TextIndexUtils.GetValueByState(timeTag.Index, colours.Yellow, colours.YellowDarker);
        }

        public static Color4 GetEditTimeTagCaretColour(this OsuColour colours)
        {
            return colours.Blue;
        }

        public static Color4 GetRecordingTimeTagCaretColour(this OsuColour colours, TimeTag timeTag)
        {
            return timeTag.Time.HasValue ? colours.Red : colours.Gray3;
        }
    }
}
