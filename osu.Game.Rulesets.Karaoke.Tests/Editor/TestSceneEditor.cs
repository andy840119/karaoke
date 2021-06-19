// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using NUnit.Framework;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Timing;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Storyboards;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    [TestFixture]
    public partial class TestSceneEditor : EditorTestScene
    {
        protected override IBeatmap CreateBeatmap(RulesetInfo ruleset) => new TestKaraokeBeatmap(ruleset);

        protected override Ruleset CreateEditorRuleset() => new KaraokeRuleset();

        protected override WorkingBeatmap CreateWorkingBeatmap(IBeatmap beatmap, Storyboard storyboard = null)
            => new CustomSkinWorkingBeatmap(beatmap, storyboard, Clock, Audio);

        public class CustomSkinWorkingBeatmap : ClockBackedTestWorkingBeatmap
        {
            public CustomSkinWorkingBeatmap(IBeatmap beatmap, Storyboard storyboard, IFrameBasedClock referenceClock, AudioManager audio)
                : base(beatmap, storyboard, referenceClock, audio)
            {
            }

            protected override Track GetBeatmapTrack()
                => TestResources.OpenTrackInfo(AudioManager, "vocal_only");

            public override Stream GetStream(string storagePath)
            {
                if(storagePath.Contains("vocal_only"))
                    return TestResources.OpenTrackResource("vocal_only");

                return null;
            }
        }
    }
}
