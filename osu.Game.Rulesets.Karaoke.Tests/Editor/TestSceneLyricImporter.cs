﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric.DragFile;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Rulesets.Karaoke.Tests.Screens;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    [TestFixture]
    public class TestSceneLyricImporter : ScreenTestScene<TestSceneLyricImporter.TestLyricImporter>
    {
        [Cached]
        private readonly OverlayColourProvider overlayColourProvider = new(OverlayColourScheme.Blue);

        [Cached]
        private readonly KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager;

        protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

        protected override TestLyricImporter CreateScreen()
        {
            string temp = TestResources.GetTestLrcForImport("light");
            return new TestLyricImporter(new FileInfo(temp));
        }

        private DialogOverlay dialogOverlay;

        public TestSceneLyricImporter()
        {
            lyricEditorConfigManager = new KaraokeRulesetLyricEditorConfigManager();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            base.Content.AddRange(new Drawable[]
            {
                Content,
                dialogOverlay = new DialogOverlay()
            });

            Dependencies.CacheAs<IDialogOverlay>(dialogOverlay);
            Dependencies.Cache(new EditorClock());
        }

        public class TestLyricImporter : LyricImporter
        {
            public TestLyricImporter(FileInfo fileInfo)
            {
                if (ScreenStack.CurrentScreen is not DragFileStepScreen dragFileSubScreen)
                    throw new ScreenStack.ScreenNotInStackException($"{nameof(DragFileStepScreen)} does not in the screen.");

                dragFileSubScreen.ImportLyricFile(fileInfo);
            }

            public void GoToStep(LyricImporterStep step)
            {
                if (ScreenStack.CurrentScreen is not ILyricImporterStepScreen lyricSubScreen)
                    return;

                if (step == lyricSubScreen.Step)
                    return;

                if (step <= lyricSubScreen.Step)
                    return;

                var totalSteps = EnumUtils.GetValues<LyricImporterStep>().Where(x => x > lyricSubScreen.Step && x <= step);

                foreach (var gotoStep in totalSteps) ScreenStack.Push(gotoStep);
            }
        }

        [Test]
        public void TestGoToStep()
        {
            var steps = EnumUtils.GetValues<LyricImporterStep>();

            foreach (var step in steps) AddStep($"go to step {Enum.GetName(typeof(LyricImporterStep), step)}", () => { Screen.GoToStep(step); });
        }
    }
}
