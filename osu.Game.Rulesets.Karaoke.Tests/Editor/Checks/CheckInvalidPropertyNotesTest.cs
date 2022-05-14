﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Tests.Beatmaps;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckInvalidPropertyNotes;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks
{
    [TestFixture]
    public class CheckInvalidPropertyNotesTest
    {
        [SetUp]
        public void Setup()
        {
            check = new CheckInvalidPropertyNotes();
        }

        private CheckInvalidPropertyNotes check;

        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(null, true)]
        public void TestCheckParentLyric(int? lyricIndex, bool expected)
        {
            var lyric = new Lyric();
            var notInBeatmapLyric = new Lyric();

            var note = new Note();

            note.ParentLyric = lyricIndex switch
            {
                0 => lyric,
                1 => notInBeatmapLyric,
                _ => note.ParentLyric
            };

            bool actual = run(lyric, note).Select(x => x.Template).OfType<IssueTemplateInvalidParentLyric>().Any();
            Assert.AreEqual(expected, actual);
        }

        private IEnumerable<Issue> run(HitObject lyric, HitObject note)
        {
            var beatmap = new Beatmap
            {
                HitObjects = new List<HitObject>
                {
                    lyric,
                    note
                }
            };
            var context = new BeatmapVerifierContext(beatmap, new TestWorkingBeatmap(beatmap));
            return check.Run(context);
        }
    }
}
