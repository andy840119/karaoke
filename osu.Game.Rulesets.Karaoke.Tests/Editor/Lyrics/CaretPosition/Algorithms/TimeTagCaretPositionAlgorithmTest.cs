﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Lyrics.CaretPosition.Algorithms
{
    [TestFixture]
    public class TimeTagCaretPositionAlgorithmTest : BaseCaretPositionAlgorithmTest<TimeTagCaretPositionAlgorithm, TimeTagCaretPosition>
    {
        private const int not_exist_tag = -1;

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, 0, true)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, 0, true)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, 0, false)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.None, 0, not_exist_tag, false)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.OnlyStartTag, 0, not_exist_tag, false)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.OnlyEndTag, 0, not_exist_tag, false)]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.None, 0, not_exist_tag, false)]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.OnlyStartTag, 0, not_exist_tag, false)]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.OnlyEndTag, 0, not_exist_tag, false)]
        public void TestPositionMovable(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, int timeTagIndex, bool movable)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createTimeTagCaretPosition(lyrics, lyricIndex, timeTagIndex);

            // Check is movable
            TestPositionMovable(lyrics, caretPosition, movable, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, 0, NOT_EXIST, not_exist_tag)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 1, 0, 0, 0)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 1, 3, 0, 3)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyStartTag, 1, 3, 0, 3)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyEndTag, 1, 3, 0, 4)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 2, 0, 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 2, 3, 0, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyStartTag, 2, 3, 0, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyEndTag, 2, 3, 0, 4)]
        public void TestMoveUp(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, int index, int newLyricIndex, int newIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createTimeTagCaretPosition(lyrics, lyricIndex, index);
            var newCaretPosition = createTimeTagCaretPosition(lyrics, newLyricIndex, newIndex);

            // Check is movable
            TestMoveUp(lyrics, caretPosition, newCaretPosition, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, 0, NOT_EXIST, not_exist_tag)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 0, 0, 1, 0)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 0, 2, 1, 2)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyStartTag, 0, 2, 1, 2)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyEndTag, 0, 2, 1, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 0, 0, 2, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 0, 2, 2, 2)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyStartTag, 0, 2, 2, 2)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyEndTag, 0, 2, 2, 3)]
        public void TestMoveDown(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, int index, int newLyricIndex, int newIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createTimeTagCaretPosition(lyrics, lyricIndex, index);
            var newCaretPosition = createTimeTagCaretPosition(lyrics, newLyricIndex, newIndex);

            // Check is movable
            TestMoveDown(lyrics, caretPosition, newCaretPosition, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, 0, NOT_EXIST, not_exist_tag)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 1, 0, 0, 4)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyStartTag, 1, 0, 0, 3)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyEndTag, 1, 0, 0, 4)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 2, 0, 0, 4)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyStartTag, 2, 0, 0, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyEndTag, 2, 0, 0, 4)]
        public void TestMoveLeft(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, int index, int newLyricIndex, int newIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createTimeTagCaretPosition(lyrics, lyricIndex, index);
            var newCaretPosition = createTimeTagCaretPosition(lyrics, newLyricIndex, newIndex);

            // Check is movable
            TestMoveLeft(lyrics, caretPosition, newCaretPosition, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, 4, NOT_EXIST, not_exist_tag)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 0, 4, 1, 0)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyStartTag, 0, 4, 1, 0)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyEndTag, 0, 4, 1, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 0, 4, 2, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyStartTag, 0, 4, 2, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyEndTag, 0, 4, 2, 3)]
        public void TestMoveRight(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, int index, int newLyricIndex, int newIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createTimeTagCaretPosition(lyrics, lyricIndex, index);
            var newCaretPosition = createTimeTagCaretPosition(lyrics, newLyricIndex, newIndex);

            // Check is movable
            TestMoveRight(lyrics, caretPosition, newCaretPosition, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, 0)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, 0)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, 4)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.None, NOT_EXIST, not_exist_tag)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.OnlyStartTag, NOT_EXIST, not_exist_tag)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.OnlyEndTag, NOT_EXIST, not_exist_tag)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 0, 0)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyStartTag, 0, 0)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyEndTag, 0, 4)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyStartTag, 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyEndTag, 0, 4)]
        public void TestMoveToFirst(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, int index)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createTimeTagCaretPosition(lyrics, lyricIndex, index);

            // Check is movable
            TestMoveToFirst(lyrics, caretPosition, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, 4)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, 3)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, 4)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.None, NOT_EXIST, not_exist_tag)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.OnlyStartTag, NOT_EXIST, not_exist_tag)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.OnlyEndTag, NOT_EXIST, not_exist_tag)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 1, 3)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyStartTag, 1, 2)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyEndTag, 1, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 2, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyStartTag, 2, 2)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyEndTag, 2, 3)]
        public void TestMoveToLast(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, int index)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createTimeTagCaretPosition(lyrics, lyricIndex, index);

            // Check is movable
            TestMoveToLast(lyrics, caretPosition, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, 0, 0)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, 0, 0)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, 0, 4)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.None, 0, NOT_EXIST, not_exist_tag)] // should not hover to the lyric if contains no time-tag in the lyric.
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.OnlyStartTag, 0, NOT_EXIST, not_exist_tag)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.OnlyEndTag, 0, NOT_EXIST, not_exist_tag)]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.None, 0, NOT_EXIST, not_exist_tag)] // should not hover to the lyric if contains no text and no time-tag in the lyric
        public void TestMoveToTarget(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, int expectedLyricIndex, int expectedTimeTagIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[lyricIndex];
            var caretPosition = createTimeTagCaretPosition(lyrics, expectedLyricIndex, expectedTimeTagIndex);

            // Check move to target position.
            TestMoveToTarget(lyrics, lyric, caretPosition, algorithms => algorithms.Mode = mode);
        }

        protected override void AssertEqual(TimeTagCaretPosition expected, TimeTagCaretPosition actual)
        {
            if (expected == null)
                Assert.IsNull(actual);
            else
            {
                Assert.AreEqual(expected.Lyric, actual.Lyric);
                Assert.AreEqual(expected.TimeTag, actual.TimeTag);
            }
        }

        private static TimeTagCaretPosition createTimeTagCaretPosition(IEnumerable<Lyric> lyrics, int lyricIndex, int timeTagIndex)
        {
            if (lyricIndex == NOT_EXIST)
                return null;

            var lyric = lyrics.ElementAtOrDefault(lyricIndex);
            var timeTag = lyric?.TimeTags?.ElementAtOrDefault(timeTagIndex);
            return new TimeTagCaretPosition(lyric, timeTag);
        }

        private static Lyric[] singleLyric => new[]
        {
            new Lyric
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(new[]
                {
                    "[0,start]:1000",
                    "[1,start]:2000",
                    "[2,start]:3000",
                    "[3,start]:4000",
                    "[3,end]:5000"
                })
            }
        };

        private static Lyric[] singleLyricWithoutTimeTag => new[]
        {
            new Lyric
            {
                Text = "カラオケ"
            }
        };

        private static Lyric[] singleLyricWithNoText => new[]
        {
            new Lyric()
        };

        private static Lyric[] twoLyricsWithText => new[]
        {
            new Lyric
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(new[]
                {
                    "[0,start]:1000",
                    "[1,start]:2000",
                    "[2,start]:3000",
                    "[3,start]:4000",
                    "[3,end]:5000"
                })
            },
            new Lyric
            {
                Text = "大好き",
                TimeTags = TestCaseTagHelper.ParseTimeTags(new[]
                {
                    "[0,start]:1000",
                    "[1,start]:2000",
                    "[2,start]:3000",
                    "[2,end]:5000"
                })
            }
        };

        private static Lyric[] threeLyricsWithSpacing => new[]
        {
            new Lyric
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(new[]
                {
                    "[0,start]:1000",
                    "[1,start]:2000",
                    "[2,start]:3000",
                    "[3,start]:4000",
                    "[3,end]:5000"
                })
            },
            new Lyric(),
            new Lyric
            {
                Text = "大好き",
                TimeTags = TestCaseTagHelper.ParseTimeTags(new[]
                {
                    "[0,start]:1000",
                    "[1,start]:2000",
                    "[2,start]:3000",
                    "[2,end]:5000"
                })
            }
        };
    }
}
