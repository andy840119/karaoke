// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Audio.Track;
using osu.Framework.Extensions;
using osu.Game.Beatmaps;
using osu.Game.Replays;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Saiten;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.Karaoke.Replays
{
    public class KaraokeAutoGeneratorBySinger : AutoGenerator
    {
        private readonly Task<Dictionary<double, float?>> readTask = null!;

        /// <summary>
        /// Using audio's voice to generate replay frames
        /// Logic is copied from <see cref="Waveform"/>
        /// </summary>
        /// <param name="beatmap"></param>
        /// <param name="data"></param>
        public KaraokeAutoGeneratorBySinger(IBeatmap beatmap, Stream? data)
            : base(beatmap)
        {
            if (data == null)
                return;

            readTask = new SaitenManager().ConvertSingerVoiceToData(data);
        }

        public override Replay Generate()
        {
            var result = readTask.GetResultSafely();
            return new Replay
            {
                Frames = getReplayFrames(result).ToList()
            };
        }

        private IEnumerable<ReplayFrame> getReplayFrames(IDictionary<double, float?> pitches)
        {
            var lastPitch = pitches.FirstOrDefault();

            foreach (var pitch in pitches)
            {
                if (pitch.Value != null)
                {
                    float scale = Beatmap.PitchToScale(pitch.Value ?? 0);
                    yield return new KaraokeReplayFrame(pitch.Key, scale);
                }
                else if (lastPitch.Value != null)
                    yield return new KaraokeReplayFrame(pitch.Key);

                lastPitch = pitch;
            }
        }
    }
}
