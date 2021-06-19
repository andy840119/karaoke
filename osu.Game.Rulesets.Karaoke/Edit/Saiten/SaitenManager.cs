// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ManagedBass;
using NWaves.Features;
using osu.Framework.Allocation;
using osu.Framework.Audio.Callbacks;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Edit.Saiten
{
    public class SaitenManager : Component
    {
        public Bindable<IDictionary<double, float?>> BindableVoiceData = new Bindable<IDictionary<double, float?>>();

        public BindableFloat BindableProgress = new BindableFloat();

        public BindableFloat PitchToScaleScale = new BindableFloat(1 / 20f)
        {
            MinValue = 0.001f,
            MaxValue = 0.3f,
        };

        public BindableFloat PitchToScalePanning = new BindableFloat(2)
        {
            MinValue = -10,
            MaxValue = 10,
        };

        [Resolved]
        private IBindable<WorkingBeatmap> beatmap { get; set; }

        public void ConvertSingerVoiceToData()
        {
            // todo : get real track name.
            var progress = new Progress<float>();
            progress.ProgressChanged += (a, b) =>
            {
                BindableProgress.Value = b;
            };

            var task = execute("vocal_only", progress);
            task.GetAwaiter().OnCompleted(() =>
            {
                BindableVoiceData.Value = task.Result;
            });

            async Task<Dictionary<double, float?>> execute(string fileName, IProgress<float> progress = null)
            {
                Dictionary<double, float?> result;

                await using (var stream = beatmap.Value.GetStream(fileName))
                {
                    result = await ConvertSingerVoiceToData(stream, progress);
                }

                return result;
            }
        }

        public Task<Dictionary<double, float?>> ConvertSingerVoiceToData(Stream track, IProgress<float> progress = null)
        {
            return Task.Run(() =>
            {
                int decodeStream;

                using (var fileCallbacks = new FileCallbacks(new DataStreamFileProcedures(track)))
                {
                    decodeStream = Bass.CreateStream(StreamSystem.NoBuffer, BassFlags.Decode | BassFlags.Float, fileCallbacks.Callbacks, fileCallbacks.Handle);
                }

                Bass.ChannelGetInfo(decodeStream, out var info);

                long totalLength = Bass.ChannelGetLength(decodeStream);
                double trackLength = Bass.ChannelBytes2Seconds(decodeStream, totalLength) * 1000;
                long length = totalLength;
                long lengthSum = 0;

                // Microphone at period 10
                int bytesPerIteration = 3276 * info.Channels * TrackBass.BYTES_PER_SAMPLE;

                var pitches = new Dictionary<double, float?>();
                float[] sampleBuffer = new float[bytesPerIteration / TrackBass.BYTES_PER_SAMPLE];

                // Read sample data
                while (length > 0)
                {
                    length = Bass.ChannelGetData(decodeStream, sampleBuffer, bytesPerIteration);
                    lengthSum += length;

                    // usually sample 1 is vocal
                    float[] channel0Sample = sampleBuffer.Where((_, i) => i % 2 == 0).ToArray();
                    //var channel1Sample = sampleBuffer.Where((x, i) => i % 2 != 0).ToArray();

                    // Convert buffer to pitch data
                    double time = lengthSum * trackLength / totalLength;
                    float pitch = Pitch.FromYin(channel0Sample, info.Frequency, low: 40, high: 1000);
                    pitches.Add(time, pitch == 0 ? default(float?) : pitch);
                }

                return pitches;
            });
        }
    }
}
