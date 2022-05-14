﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Lyrics.States
{
    [HeadlessTest]
    public class LyricCaretStateTest : OsuTestScene
    {
        private readonly IBeatmap beatmap;
        private ILyricEditorState state;
        private LyricCaretState lyricCaretState;

        public LyricCaretStateTest()
        {
            beatmap = new KaraokeBeatmap
            {
                BeatmapInfo =
                {
                    Ruleset = new KaraokeRuleset().RulesetInfo
                },
                HitObjects = new List<KaraokeHitObject>
                {
                    new Lyric
                    {
                        Text = "First lyric"
                    },
                    new Lyric
                    {
                        Text = "Second lyric"
                    },
                    new Lyric
                    {
                        Text = "Third lyric"
                    }
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Dependencies.Cache(new EditorBeatmap(beatmap));
            Dependencies.Cache(new EditorClock());
            Dependencies.CacheAs(state = new TestLyricEditorState());
            Dependencies.CacheAs<ITimeTagModeState>(new TimeTagModeState());
            Dependencies.Cache(new KaraokeRulesetLyricEditorConfigManager());

            var lyricsProvider = new LyricsProvider();
            Dependencies.CacheAs<ILyricsProvider>(lyricsProvider);

            Children = new Drawable[]
            {
                lyricsProvider,
                lyricCaretState = new LyricCaretState()
            };
        }

        private void changeMode(LyricEditorMode mode)
        {
            AddStep("Switch to edit mode", () =>
            {
                state.SwitchMode(mode);
            });
        }

        private void movingCaret(MovingCaretAction action)
        {
            AddStep("Moving caret by action", () =>
            {
                lyricCaretState.MoveCaret(action);
            });
        }

        private void movingCaretByLyric(Lyric lyric)
        {
            AddStep("Moving caret by action", () =>
            {
                lyricCaretState.MoveCaretToTargetPosition(lyric);
            });
        }

        private void movingCaretByLyric(ICaretPosition caretPosition, bool exceptSuccess = true)
        {
            AddStep("Moving caret by caret position", () =>
            {
                try
                {
                    lyricCaretState.MoveCaretToTargetPosition(caretPosition);
                    Assert.IsTrue(exceptSuccess);
                }
                catch
                {
                    Assert.IsFalse(exceptSuccess);
                }
            });
        }

        private void movingHoverCaretByLyric(ICaretPosition caretPosition, bool exceptSuccess = true)
        {
            AddStep("Moving hover caret by caret position", () =>
            {
                try
                {
                    lyricCaretState.MoveHoverCaretToTargetPosition(caretPosition);
                    Assert.IsTrue(exceptSuccess);
                }
                catch
                {
                    Assert.IsFalse(exceptSuccess);
                }
            });
        }

        private Lyric getLyricFromBeatmap(int index)
        {
            return beatmap.HitObjects[index] as Lyric;
        }

        private void assertCaretPosition(Action<ICaretPosition> caretPosition)
        {
            AddStep("Assert caret position", () =>
            {
                caretPosition.Invoke(lyricCaretState.BindableCaretPosition.Value);
            });
        }

        private void assertHoverCaretPosition(Action<ICaretPosition> caretPosition)
        {
            AddStep("Assert hover caret position", () =>
            {
                caretPosition.Invoke(lyricCaretState.BindableHoverCaretPosition.Value);
            });
        }

        private class TestLyricEditorState : ILyricEditorState
        {
            public IBindable<LyricEditorMode> BindableMode => bindableMode;

            public LyricEditorMode Mode => bindableMode.Value;
            private readonly Bindable<LyricEditorMode> bindableMode = new();

            public void SwitchMode(LyricEditorMode mode)
            {
                bindableMode.Value = mode;
            }

            public void NavigateToFix(LyricEditorMode mode)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void MovingCaretByCaretPositionWithEditMode()
        {
            changeMode(LyricEditorMode.Singer);

            var targetLyric = getLyricFromBeatmap(1);
            movingCaretByLyric(new NavigateCaretPosition(targetLyric));

            // yes, should change the position if contains algorithm.
            assertCaretPosition(Assert.IsInstanceOf<NavigateCaretPosition>);
            assertHoverCaretPosition(Assert.IsNull);
        }

        [Test]
        public void MovingCaretByCaretPositionWithViewMode()
        {
            changeMode(LyricEditorMode.View);

            var targetLyric = getLyricFromBeatmap(1);
            movingCaretByLyric(new NavigateCaretPosition(targetLyric));

            // should not change the caret position if algorithm is null.
            assertCaretPosition(Assert.IsNull);
            assertHoverCaretPosition(Assert.IsNull);
        }

        [Test]
        public void MovingCaretByCaretPositionWithWrongCaretPosition()
        {
            changeMode(LyricEditorMode.Manage);

            var targetLyric = getLyricFromBeatmap(1);

            // should throw the exception if caret position type not match.
            movingCaretByLyric(new NavigateCaretPosition(targetLyric), false);
        }

        [Test]
        public void MovingCaretByLyricWithEditMode()
        {
            changeMode(LyricEditorMode.Manage);

            var targetLyric = getLyricFromBeatmap(1);
            movingCaretByLyric(targetLyric);

            // get the action
            assertCaretPosition(Assert.IsInstanceOf<TextCaretPosition>);
            assertHoverCaretPosition(Assert.IsNull);
        }

        [Test]
        public void MovingCaretByLyricWithViewMode()
        {
            changeMode(LyricEditorMode.View);

            var targetLyric = getLyricFromBeatmap(1);
            movingCaretByLyric(targetLyric);

            // get the action
            assertCaretPosition(Assert.IsNull);
            assertHoverCaretPosition(Assert.IsNull);
        }

        [Test]
        public void MovingCaretWithEditMode()
        {
            changeMode(LyricEditorMode.Manage);
            movingCaret(MovingCaretAction.First);

            // get the action
            assertCaretPosition(Assert.IsInstanceOf<TextCaretPosition>);
            assertHoverCaretPosition(Assert.IsNull);
        }

        [Test]
        public void MovingCaretWithViewMode()
        {
            changeMode(LyricEditorMode.View);
            movingCaret(MovingCaretAction.First);

            // get the action
            assertCaretPosition(Assert.IsNull);
            assertHoverCaretPosition(Assert.IsNull);
        }

        [Test]
        public void MovingHoverCaretByCaretPositionWithEditMode()
        {
            changeMode(LyricEditorMode.Singer);

            var firstLyric = getLyricFromBeatmap(0);
            var targetLyric = getLyricFromBeatmap(1);
            movingHoverCaretByLyric(new NavigateCaretPosition(targetLyric));

            // because switch to the singer lyric, so current caret position will at the first lyric.
            assertCaretPosition(Assert.IsInstanceOf<NavigateCaretPosition>);
            assertCaretPosition(x => Assert.AreEqual(firstLyric, x.Lyric));

            // yes, should change the position if contains algorithm.
            assertHoverCaretPosition(Assert.IsInstanceOf<NavigateCaretPosition>);
            assertHoverCaretPosition(x => Assert.AreEqual(targetLyric, x.Lyric));
        }

        [Test]
        public void MovingHoverCaretByCaretPositionWithViewMode()
        {
            changeMode(LyricEditorMode.View);

            var targetLyric = getLyricFromBeatmap(1);
            movingHoverCaretByLyric(new NavigateCaretPosition(targetLyric));

            // should not change the caret position if algorithm is null.
            assertCaretPosition(Assert.IsNull);
            assertHoverCaretPosition(Assert.IsNull);
        }

        [Test]
        public void MovingHoverCaretByCaretPositionWithWrongCaretPosition()
        {
            changeMode(LyricEditorMode.Manage);

            var targetLyric = getLyricFromBeatmap(1);

            // should throw the exception if caret position type not match.
            movingHoverCaretByLyric(new NavigateCaretPosition(targetLyric), false);
        }

        [Test]
        public void TestChangeFromEditModeToEditMode()
        {
            // change from edit mode to view mode for checking that caret position should be clear.
            changeMode(LyricEditorMode.Manage);
            changeMode(LyricEditorMode.EditRuby);

            // get the action
            assertCaretPosition(Assert.IsInstanceOf<NavigateCaretPosition>);
            assertHoverCaretPosition(Assert.IsNull);
        }

        [Test]
        public void TestChangeFromEditModeToViewMode()
        {
            // change from edit mode to view mode for checking that caret position should be clear.
            changeMode(LyricEditorMode.Manage);
            changeMode(LyricEditorMode.View);

            // get the action
            assertCaretPosition(Assert.IsNull);
            assertHoverCaretPosition(Assert.IsNull);
        }

        [Test]
        public void TestChangeFromViewModeToEditMode()
        {
            // change from view mode to another mode that contains algorithm.
            changeMode(LyricEditorMode.View);
            changeMode(LyricEditorMode.Manage);

            // get the action
            assertCaretPosition(Assert.IsNotNull);
            assertHoverCaretPosition(Assert.IsNull);
        }
    }
}
