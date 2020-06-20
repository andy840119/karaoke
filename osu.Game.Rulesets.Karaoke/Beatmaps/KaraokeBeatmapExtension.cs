// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Beatmaps
{
    public static class KaraokeBeatmapExtension
    {
        public static bool IsScorable(this IBeatmap beatmap)
        {
            if (beatmap is not KaraokeBeatmap karaokeBeatmap)
            {
                // we should throw invalidate exception here but it will cause test case failed.
                // because beatmap in the working beatmap in test case not always be karaoke beatmap class.
                return false;
            }

            return karaokeBeatmap.Scorable;
        }

        public static IList<CultureInfo> AvailableTranslates(this IBeatmap beatmap) => (beatmap as KaraokeBeatmap)?.AvailableTranslates ?? new List<CultureInfo>();

        public static bool AnyTranslate(this IBeatmap beatmap) => beatmap.AvailableTranslates().Any();

        public static PitchShifting GetBeatmapPitchSetting(this IBeatmap beatmap) => new PitchShifting
        {
            Shifting = -7,
            Scale = 0.05f,
        };

        /// <summary>
        /// Apply pitch to scale by beatmap's setting
        /// </summary>
        /// <param name="beatmap"></param>
        /// <param name="pitch"></param>
        /// <returns></returns>
        public static float PitchToScale(this IBeatmap beatmap, float pitch)
        {
            var setting = beatmap.GetBeatmapPitchSetting();
            return pitch * setting.ScaleShifting(pitch);
        }

        public static IList<Singer> GetSingers(this IBeatmap beatmap) => (beatmap as KaraokeBeatmap)?.Singers ?? new List<Singer>();
    }
}
