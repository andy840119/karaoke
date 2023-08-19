// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.CaretPosition;

[TestFixture(typeof(CreateRubyTagCaretPosition))]
[TestFixture(typeof(CuttingCaretPosition))]
[TestFixture(typeof(TimeTagCaretPosition))]
[TestFixture(typeof(TimeTagIndexCaretPosition))]
[TestFixture(typeof(TypingCaretPosition))]
public class IndexCaretPositionTest<TIndexCaretPosition> where TIndexCaretPosition : IIndexCaretPosition
{
    [Test]
    public void CompareWithLargerIndex()
    {
        var lyric = createSampleLyric();

        var caretPosition = createSmallerCaretPosition(lyric);
        var comparedCaretPosition = createBiggerCaretPosition(lyric);

        Assert.IsTrue(caretPosition < comparedCaretPosition);
        Assert.IsTrue(caretPosition <= comparedCaretPosition);
        Assert.IsFalse(caretPosition >= comparedCaretPosition);
        Assert.IsFalse(caretPosition > comparedCaretPosition);
    }

    [Test]
    public void CompareEqualIndex()
    {
        var lyric = createSampleLyric();

        var caretPosition = createSmallerCaretPosition(lyric);
        var comparedCaretPosition = createSmallerCaretPosition(lyric);

        Assert.IsFalse(caretPosition < comparedCaretPosition);
        Assert.IsTrue(caretPosition <= comparedCaretPosition);
        Assert.IsTrue(caretPosition >= comparedCaretPosition);
        Assert.IsFalse(caretPosition > comparedCaretPosition);
    }

    [Test]
    public void CompareWithSmallerIndex()
    {
        var lyric = createSampleLyric();

        var caretPosition = createBiggerCaretPosition(lyric);
        var comparedCaretPosition = createSmallerCaretPosition(lyric);

        Assert.IsFalse(caretPosition < comparedCaretPosition);
        Assert.IsFalse(caretPosition <= comparedCaretPosition);
        Assert.IsTrue(caretPosition >= comparedCaretPosition);
        Assert.IsTrue(caretPosition > comparedCaretPosition);
    }

    [Test]
    public void CompareWithDifferentLyric()
    {
        var lyric1 = createSampleLyric();
        var lyric2 = createSampleLyric();

        var caretPosition = createBiggerCaretPosition(lyric1);
        var comparedCaretPosition = createSmallerCaretPosition(lyric2);

        Assert.Throws<InvalidOperationException>(() => caretPosition.CompareTo(comparedCaretPosition));
    }

    [Test]
    public void CompareDifferentType()
    {
        var lyric = createSampleLyric();

        var caretPosition = createBiggerCaretPosition(lyric);
        var comparedCaretPosition = new FakeCaretPosition(lyric);

        Assert.Throws<InvalidOperationException>(() => caretPosition.CompareTo(comparedCaretPosition));
    }

    [Test]
    public void CreateInvalidCaretPosition()
    {
        var lyric = createSampleLyric();

        Assert.Throws<InvalidOperationException>(() => createInvalidCaretPosition(lyric));
    }

    private static Lyric createSampleLyric()
    {
        return new Lyric
        {
            Text = "カラオケ",
            TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1000", "[3,end]:4000" }),
            RubyTags = TestCaseTagHelper.ParseRubyTags(new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" }),
            RomajiTags = TestCaseTagHelper.ParseRomajiTags(new[] { "[0]:ka", "[1]:ra", "[2]:o", "[3]:ke" }),
        };
    }

    private static IIndexCaretPosition createSmallerCaretPosition(Lyric lyric) =>
        typeof(TIndexCaretPosition) switch
        {
            Type t when t == typeof(CreateRubyTagCaretPosition) => new CreateRubyTagCaretPosition(lyric, 0),
            Type t when t == typeof(CuttingCaretPosition) => new CuttingCaretPosition(lyric, 0),
            Type t when t == typeof(TimeTagCaretPosition) => new TimeTagCaretPosition(lyric, lyric.TimeTags.First()),
            Type t when t == typeof(TimeTagIndexCaretPosition) => new TimeTagIndexCaretPosition(lyric, 0),
            Type t when t == typeof(TypingCaretPosition) => new TypingCaretPosition(lyric, 0),
            _ => throw new NotSupportedException(),
        };

    private static IIndexCaretPosition createBiggerCaretPosition(Lyric lyric) =>
        typeof(TIndexCaretPosition) switch
        {
            Type t when t == typeof(CreateRubyTagCaretPosition) => new CreateRubyTagCaretPosition(lyric, 3),
            Type t when t == typeof(CuttingCaretPosition) => new CuttingCaretPosition(lyric, 4),
            Type t when t == typeof(TimeTagCaretPosition) => new TimeTagCaretPosition(lyric, lyric.TimeTags.Last()),
            Type t when t == typeof(TimeTagIndexCaretPosition) => new TimeTagIndexCaretPosition(lyric, 3),
            Type t when t == typeof(TypingCaretPosition) => new TypingCaretPosition(lyric, 4),
            _ => throw new NotSupportedException(),
        };

    private static IIndexCaretPosition createInvalidCaretPosition(Lyric lyric) =>
        typeof(TIndexCaretPosition) switch
        {
            Type t when t == typeof(CreateRubyTagCaretPosition) => new CreateRubyTagCaretPosition(lyric, -1),
            Type t when t == typeof(CuttingCaretPosition) => new CuttingCaretPosition(lyric, -1),
            Type t when t == typeof(TimeTagCaretPosition) => new TimeTagCaretPosition(lyric, new TimeTag(new TextIndex())),
            Type t when t == typeof(TimeTagIndexCaretPosition) => new TimeTagIndexCaretPosition(lyric, -1),
            Type t when t == typeof(TypingCaretPosition) => new TypingCaretPosition(lyric, -1),
            _ => throw new NotSupportedException(),
        };

    private struct FakeCaretPosition : IIndexCaretPosition
    {
        public FakeCaretPosition(Lyric lyric)
        {
            Lyric = lyric;
        }

        public Lyric Lyric { get; }

        public int CompareTo(IIndexCaretPosition? other)
        {
            throw new NotImplementedException();
        }
    }
}
