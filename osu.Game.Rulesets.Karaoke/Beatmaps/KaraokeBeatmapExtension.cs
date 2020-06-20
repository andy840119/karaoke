// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps
{
    public static class KaraokeBeatmapExtension
    {
        public static bool IsScorable(this IBeatmap beatmap) => beatmap?.HitObjects.OfType<Note>().Any(x => x.Display) ?? false;

        public static CultureInfo[] AvailableTranslates(this IBeatmap beatmap) => (beatmap as KaraokeBeatmap)?.AvailableTranslates ?? new CultureInfo[] { };

        public static bool AnyTranslate(this IBeatmap beatmap) => beatmap?.AvailableTranslates()?.Any() ?? false;

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

        public static Singer[] GetSingers(this IBeatmap beatmap) => (beatmap as KaraokeBeatmap)?.Singers;
    }
}
