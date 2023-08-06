// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Generator;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics;

public partial class LyricTimeTagsChangeHandlerTest : LyricPropertyChangeHandlerTest<LyricTimeTagsChangeHandler>
{
    protected override bool IncludeAutoGenerator => true;

    #region TimeTag

    [Test]
    public void TestAutoGenerateTimeTags()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            Language = new CultureInfo(17),
        });

        TriggerHandlerChanged(c => c.AutoGenerate(TimeTagGeneratorType.TimeTag));

        AssertSelectedHitObject(h =>
        {
            Assert.AreEqual(5, h.TimeTags.Count);
        });
    }

    [Test]
    public void TestAutoGenerateRomajis()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            Language = new CultureInfo(17),
            TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" }),
        });

        TriggerHandlerChanged(c => c.AutoGenerate(TimeTagGeneratorType.Romaji));

        AssertSelectedHitObject(h =>
        {
            Assert.AreEqual("karaoke", h.TimeTags[0].RomajiText);
            Assert.AreEqual(string.Empty, h.TimeTags[1].RomajiText);
            Assert.AreEqual(string.Empty, h.TimeTags[2].RomajiText);
            Assert.AreEqual(string.Empty, h.TimeTags[3].RomajiText);
            Assert.AreEqual(string.Empty, h.TimeTags[3].RomajiText);
        });
    }

    [Test]
    public void TestAutoGenerateTimeTagsWithNonSupportedLyric()
    {
        PrepareHitObjects(() => new[]
        {
            new Lyric
            {
                Text = "カラオケ",
            },
            new Lyric
            {
                Text = string.Empty,
            },
            new Lyric
            {
                Text = string.Empty,
                Language = new CultureInfo(17),
            },
        });

        TriggerHandlerChangedWithException<GeneratorNotSupportedException>(c => c.AutoGenerate(TimeTagGeneratorType.TimeTag));
    }

    [Test]
    public void TestAutoGenerateRomajisWithNonSupportedLyric()
    {
        PrepareHitObjects(() => new[]
        {
            new Lyric
            {
                Text = "カラオケ",
            },
        });

        TriggerHandlerChangedWithException<GeneratorNotSupportedException>(c => c.AutoGenerate(TimeTagGeneratorType.Romaji));
    }

    #endregion

    [Test]
    public void TestSetTimeTagTime()
    {
        var timeTag = new TimeTag(new TextIndex(), 1000);
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                timeTag,
            },
        });

        TriggerHandlerChanged(c => c.SetTimeTagTime(timeTag, 2000));

        AssertSelectedHitObject(_ =>
        {
            Assert.AreEqual(2000, timeTag.Time);
        });
    }

    [Test]
    public void TestSetTimeTagInitialRomaji()
    {
        var timeTag = new TimeTag(new TextIndex(), 1000);
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                timeTag,
            },
        });

        TriggerHandlerChanged(c => c.SetTimeTagInitialRomaji(timeTag, true));

        AssertSelectedHitObject(_ =>
        {
            Assert.AreEqual(true, timeTag.InitialRomaji);
        });
    }

    [Test]
    public void TestSetTimeTagRomajiText()
    {
        var timeTag = new TimeTag(new TextIndex(), 1000);
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                timeTag,
            },
        });

        TriggerHandlerChanged(c => c.SetTimeTagRomajiText(timeTag, "karaoke"));

        AssertSelectedHitObject(_ =>
        {
            Assert.AreEqual("karaoke", timeTag.RomajiText);
        });

        TriggerHandlerChanged(c => c.SetTimeTagRomajiText(timeTag, "  "));

        AssertSelectedHitObject(_ =>
        {
            Assert.AreEqual(string.Empty, timeTag.RomajiText);
        });
    }

    [Test]
    public void TestShiftingTimeTagTime()
    {
        var timeTag = new TimeTag(new TextIndex());
        var timeTagWithTime = new TimeTag(new TextIndex(), 1000);
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                timeTag,
                timeTagWithTime,
            },
        });

        TriggerHandlerChanged(c => c.ShiftingTimeTagTime(new[] { timeTag, timeTagWithTime }, 2000));

        // use this temp way to trigger transaction count increase.
        AddStep("Prepare testing beatmap", () =>
        {
            var editorBeatmap = Dependencies.Get<EditorBeatmap>();
            editorBeatmap.PerformOnSelection(h =>
            {
                editorBeatmap.Update(h);
            });
        });

        AssertSelectedHitObject(_ =>
        {
            Assert.AreEqual(null, timeTag.Time);
            Assert.AreEqual(3000, timeTagWithTime.Time);
        });
    }

    [Test]
    public void TestClearTimeTagTime()
    {
        var timeTag = new TimeTag(new TextIndex(), 1000);
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                timeTag,
            },
        });

        TriggerHandlerChanged(c => c.ClearTimeTagTime(timeTag));

        AssertSelectedHitObject(_ =>
        {
            Assert.IsNull(timeTag.Time);
        });
    }

    [Test]
    public void TestClearAllTimeTagTime()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex()), // without time.
                new TimeTag(new TextIndex(), 1000),
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 3000),
            },
        });

        TriggerHandlerChanged(c => c.ClearAllTimeTagTime());

        AssertSelectedHitObject(h =>
        {
            Assert.IsTrue(h.TimeTags.All(x => x.Time == null));
        });
    }

    [Test]
    public void TestAdd()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
        });

        TriggerHandlerChanged(c => c.Add(new TimeTag(new TextIndex(), 1000)));

        AssertSelectedHitObject(h =>
        {
            Assert.AreEqual(1, h.TimeTags.Count);
            Assert.AreEqual(1000, h.TimeTags[0].Time);
        });
    }

    [Test]
    public void TestAddRange()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
        });

        TriggerHandlerChanged(c => c.AddRange(new[] { new TimeTag(new TextIndex(), 1000) }));

        AssertSelectedHitObject(h =>
        {
            Assert.AreEqual(1, h.TimeTags.Count);
            Assert.AreEqual(1000, h.TimeTags[0].Time);
        });
    }

    [Test]
    public void TestRemove()
    {
        var removedTag = new TimeTag(new TextIndex(), 1000);

        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                removedTag,
            },
        });

        TriggerHandlerChanged(c => c.Remove(removedTag));

        AssertSelectedHitObject(h =>
        {
            Assert.IsEmpty(h.TimeTags);
        });
    }

    [Test]
    public void TestRemoveRange()
    {
        var removedTag = new TimeTag(new TextIndex(), 1000);

        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                removedTag,
            },
        });

        TriggerHandlerChanged(c => c.RemoveRange(new[] { removedTag }));

        AssertSelectedHitObject(h =>
        {
            Assert.IsEmpty(h.TimeTags);
        });
    }

    [Test]
    public void TestAddByPosition()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
        });

        TriggerHandlerChanged(c => c.AddByPosition(new TextIndex(3, TextIndex.IndexState.End)));

        AssertSelectedHitObject(h =>
        {
            Assert.AreEqual(1, h.TimeTags.Count);

            var actualTimeTag = h.TimeTags[0];
            Assert.AreEqual(new TextIndex(3, TextIndex.IndexState.End), actualTimeTag.Index);
            Assert.IsNull(actualTimeTag.Time);
        });
    }

    [Test]
    public void TestRemoveByPosition()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 4000),
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End)),
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 5000),
            },
        });

        TriggerHandlerChanged(c => c.RemoveByPosition(new TextIndex(3, TextIndex.IndexState.End)));

        AssertSelectedHitObject(h =>
        {
            Assert.AreEqual(2, h.TimeTags.Count);

            // should delete the min time of the time-tag
            var actualTimeTag = h.TimeTags[0];
            Assert.AreEqual(new TextIndex(3, TextIndex.IndexState.End), actualTimeTag.Index);
            Assert.AreEqual(4000, actualTimeTag.Time);
        });
    }

    [Test]
    public void TestRemoveByPositionCase2()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 5000),
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 4000),
            },
        });

        TriggerHandlerChanged(c => c.RemoveByPosition(new TextIndex(3, TextIndex.IndexState.End)));

        AssertSelectedHitObject(h =>
        {
            Assert.AreEqual(1, h.TimeTags.Count);

            // should delete the min time of the time-tag
            var actualTimeTag = h.TimeTags[0];
            Assert.AreEqual(new TextIndex(3, TextIndex.IndexState.End), actualTimeTag.Index);
            Assert.AreEqual(5000, actualTimeTag.Time);
        });
    }

    [TestCase(ShiftingDirection.Left, ShiftingType.Index, 1)]
    [TestCase(ShiftingDirection.Left, ShiftingType.State, 2)]
    [TestCase(ShiftingDirection.Right, ShiftingType.State, 2)]
    [TestCase(ShiftingDirection.Right, ShiftingType.Index, 3)]
    public void TestShifting(ShiftingDirection direction, ShiftingType type, int expectedIndex)
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(1)),
                new TimeTag(new TextIndex(1, TextIndex.IndexState.End)),
                new TimeTag(new TextIndex(2), 4000), // target.
                new TimeTag(new TextIndex(2, TextIndex.IndexState.End)),
                new TimeTag(new TextIndex(3)),
            },
        });

        TriggerHandlerChanged(c =>
        {
            var lyric = Dependencies.Get<EditorBeatmap>().HitObjects.OfType<Lyric>().First();
            var targetTimeTag = lyric.TimeTags[2];
            var actualTimeTag = c.Shifting(targetTimeTag, direction, type);

            Assert.AreEqual(expectedIndex, lyric.TimeTags.IndexOf(actualTimeTag));

            // the property should be the same
            Assert.AreEqual(targetTimeTag.Time, actualTimeTag.Time);
        });
    }

    [TestCase(ShiftingDirection.Left, ShiftingType.Index, 0)]
    [TestCase(ShiftingDirection.Left, ShiftingType.State, 0)]
    [TestCase(ShiftingDirection.Right, ShiftingType.State, 0)]
    [TestCase(ShiftingDirection.Right, ShiftingType.Index, 0)]
    public void TestShiftingToFirst(ShiftingDirection direction, ShiftingType type, int expectedIndex)
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(1)), // target.
                new TimeTag(new TextIndex(3)),
            },
        });

        TriggerHandlerChanged(c =>
        {
            var lyric = Dependencies.Get<EditorBeatmap>().HitObjects.OfType<Lyric>().First();
            var targetTimeTag = lyric.TimeTags[0];
            var actualTimeTag = c.Shifting(targetTimeTag, direction, type);

            Assert.AreEqual(expectedIndex, lyric.TimeTags.IndexOf(actualTimeTag));

            // the property should be the same
            Assert.AreEqual(targetTimeTag.Time, actualTimeTag.Time);
        });
    }

    [TestCase(ShiftingDirection.Left, ShiftingType.Index, 1)]
    [TestCase(ShiftingDirection.Left, ShiftingType.State, 1)]
    [TestCase(ShiftingDirection.Right, ShiftingType.State, 1)]
    [TestCase(ShiftingDirection.Right, ShiftingType.Index, 1)]
    public void TestShiftingToLast(ShiftingDirection direction, ShiftingType type, int expectedIndex)
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(0)),
                new TimeTag(new TextIndex(2)), // target.
            },
        });

        TriggerHandlerChanged(c =>
        {
            var lyric = Dependencies.Get<EditorBeatmap>().HitObjects.OfType<Lyric>().First();
            var targetTimeTag = lyric.TimeTags[1];
            var actualTimeTag = c.Shifting(targetTimeTag, direction, type);

            Assert.AreEqual(expectedIndex, lyric.TimeTags.IndexOf(actualTimeTag));

            // the property should be the same
            Assert.AreEqual(targetTimeTag.Time, actualTimeTag.Time);
        });
    }

    [TestCase(ShiftingDirection.Left, ShiftingType.Index, 1)]
    [TestCase(ShiftingDirection.Left, ShiftingType.State, 1)]
    [TestCase(ShiftingDirection.Right, ShiftingType.State, 1)]
    [TestCase(ShiftingDirection.Right, ShiftingType.Index, 1)]
    public void TestShiftingWithNoDuplicatedTimeTag(ShiftingDirection direction, ShiftingType type, int expectedIndex)
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(0)),
                new TimeTag(new TextIndex(2), 4000), // target.
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End)),
            },
        });

        TriggerHandlerChanged(c =>
        {
            var lyric = Dependencies.Get<EditorBeatmap>().HitObjects.OfType<Lyric>().First();
            var targetTimeTag = lyric.TimeTags[1];
            var actualTimeTag = c.Shifting(targetTimeTag, direction, type);

            Assert.AreEqual(expectedIndex, lyric.TimeTags.IndexOf(actualTimeTag));

            // the property should be the same
            Assert.AreEqual(targetTimeTag.Time, actualTimeTag.Time);
        });
    }

    [TestCase(ShiftingDirection.Left, ShiftingType.Index, 0)]
    [TestCase(ShiftingDirection.Left, ShiftingType.State, 0)]
    [TestCase(ShiftingDirection.Right, ShiftingType.State, 0)]
    [TestCase(ShiftingDirection.Right, ShiftingType.Index, 0)]
    public void TestShiftingWithOneTimeTag(ShiftingDirection direction, ShiftingType type, int expectedIndex)
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(2), 4000), // target.
            },
        });

        TriggerHandlerChanged(c =>
        {
            var lyric = Dependencies.Get<EditorBeatmap>().HitObjects.OfType<Lyric>().First();
            var targetTimeTag = lyric.TimeTags[0];
            var actualTimeTag = c.Shifting(targetTimeTag, direction, type);

            Assert.AreEqual(expectedIndex, lyric.TimeTags.IndexOf(actualTimeTag));

            // the property should be the same
            Assert.AreEqual(targetTimeTag.Time, actualTimeTag.Time);
        });
    }

    [Ignore("Will be implement if it will increase better UX.")]
    [TestCase(ShiftingDirection.Left, ShiftingType.State, 1)]
    [TestCase(ShiftingDirection.Left, ShiftingType.Index, 1)]
    [TestCase(ShiftingDirection.Right, ShiftingType.State, 3)]
    [TestCase(ShiftingDirection.Right, ShiftingType.Index, 3)]
    public void TestShiftingWithSameTextTag(ShiftingDirection direction, ShiftingType type, int expectedIndex)
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(2), 3000),
                new TimeTag(new TextIndex(2), 4000), // target.
                new TimeTag(new TextIndex(2), 5000),
            },
        });

        TriggerHandlerChanged(c =>
        {
            var lyric = Dependencies.Get<EditorBeatmap>().HitObjects.OfType<Lyric>().First();
            var targetTimeTag = lyric.TimeTags[1];
            var actualTimeTag = c.Shifting(targetTimeTag, direction, type);

            Assert.AreEqual(expectedIndex, lyric.TimeTags.IndexOf(actualTimeTag));

            // the property should be the same
            Assert.AreEqual(targetTimeTag.Time, actualTimeTag.Time);
        });
    }

    [TestCase(TextIndex.IndexState.Start, ShiftingDirection.Left, ShiftingType.Index)]
    [TestCase(TextIndex.IndexState.Start, ShiftingDirection.Left, ShiftingType.State)]
    [TestCase(TextIndex.IndexState.Start, ShiftingDirection.Right, ShiftingType.Index)]
    [TestCase(TextIndex.IndexState.End, ShiftingDirection.Left, ShiftingType.Index)]
    [TestCase(TextIndex.IndexState.End, ShiftingDirection.Right, ShiftingType.State)]
    [TestCase(TextIndex.IndexState.End, ShiftingDirection.Right, ShiftingType.Index)]
    public void TestShiftingException(TextIndex.IndexState state, ShiftingDirection direction, ShiftingType type)
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "-",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(0, state), 5000), // target.
            },
        });

        // will have exception because the time-tag cannot move right.
        TriggerHandlerChanged(c =>
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var lyric = Dependencies.Get<EditorBeatmap>().HitObjects.OfType<Lyric>().First();
                var targetTimeTag = lyric.TimeTags[0];
                c.Shifting(targetTimeTag, direction, type);
            });
        });
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestWithReferenceLyric(bool syncTimeTag)
    {
        var lyric = PrepareLyricWithSyncConfig(new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(), 1000),
            },
        }, new SyncLyricConfig
        {
            SyncTimeTagProperty = syncTimeTag,
        });

        // should add the time-tag by hand because it does not sync from thr referenced lyric.
        if (!syncTimeTag)
        {
            lyric.TimeTags = new[]
            {
                new TimeTag(new TextIndex(), 2000),
            };
        }

        var timeTag = lyric.TimeTags.First();

        if (syncTimeTag)
        {
            TriggerHandlerChangedWithException<ChangeForbiddenException>(c => c.SetTimeTagTime(timeTag, 2000));
        }
        else
        {
            TriggerHandlerChanged(c => c.SetTimeTagTime(timeTag, 2000));
        }
    }
}
