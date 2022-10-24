﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Components.Cursor;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    [TestFixture]
    public class TestSceneInvalidLyricToolTip : OsuTestScene
    {
        private InvalidLyricToolTip toolTip = null!;

        [SetUp]
        public void SetUp() => Schedule(() =>
        {
            Child = toolTip = new InvalidLyricToolTip
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };
            toolTip.Show();
        });

        [Test]
        public void TestValidLyric()
        {
            setTooltip("valid lyric");
        }

        [Test]
        public void TestTimeInvalidLyric()
        {
            setTooltip("overlapping time", new TestLyricTimeIssue(new[]
            {
                TimeInvalid.Overlapping,
            }));

            setTooltip("start time invalid", new TestLyricTimeIssue(new[]
            {
                TimeInvalid.StartTimeInvalid,
            }));

            setTooltip("end time invalid", new TestLyricTimeIssue(new[]
            {
                TimeInvalid.EndTimeInvalid,
            }));
        }

        [Test]
        public void TestTimeTagInvalidLyric()
        {
            setTooltip("time tag out of range", new TestTimeTagIssue(
                new Dictionary<TimeTagInvalid, TimeTag[]>
                {
                    {
                        TimeTagInvalid.OutOfRange,
                        new[]
                        {
                            new TimeTag(new TextIndex(2))
                        }
                    },
                }));

            setTooltip("time tag overlapping", new TestTimeTagIssue(new Dictionary<TimeTagInvalid, TimeTag[]>
            {
                {
                    TimeTagInvalid.Overlapping,
                    new[]
                    {
                        new TimeTag(new TextIndex(2))
                    }
                }
            }));

            setTooltip("time tag with no time", new TestTimeTagIssue(new Dictionary<TimeTagInvalid, TimeTag[]>
            {
                {
                    TimeTagInvalid.EmptyTime,
                    new[]
                    {
                        new TimeTag(new TextIndex(2))
                    }
                }
            }));

            setTooltip("missing start time-tag", new TestTimeTagIssue(new Dictionary<TimeTagInvalid, TimeTag[]>(), true));
            setTooltip("missing end time-tag", new TestTimeTagIssue(new Dictionary<TimeTagInvalid, TimeTag[]>(), false, true));
            setTooltip("missing start and end time-tag", new TestTimeTagIssue(new Dictionary<TimeTagInvalid, TimeTag[]>(), true, true));
        }

        [Test]
        public void TestMultiInvalidLyric()
        {
            setTooltip("multi property is invalid", new TestLyricTimeIssue(new[]
            {
                TimeInvalid.Overlapping,
                TimeInvalid.StartTimeInvalid,
            }), new TestTimeTagIssue(new Dictionary<TimeTagInvalid, TimeTag[]>
            {
                {
                    TimeTagInvalid.OutOfRange,
                    new[]
                    {
                        new TimeTag(new TextIndex(2))
                    }
                },
            }));
        }

        private void setTooltip(string testName, params Issue[] issues)
        {
            AddStep(testName, () =>
            {
                toolTip.SetContent(issues);
            });
        }

        internal class Check : ICheck
        {
            public IEnumerable<Issue> Run(BeatmapVerifierContext context)
            {
                throw new NotImplementedException();
            }

            public CheckMetadata Metadata { get; } = null!;

            public IEnumerable<IssueTemplate> PossibleTemplates { get; } = null!;
        }

        internal class TestIssueTemplate : IssueTemplate
        {
            public TestIssueTemplate()
                : base(new Check(), IssueType.Error, string.Empty)
            {
            }
        }

        internal class TestLyricTimeIssue : LyricTimeIssue
        {
            public TestLyricTimeIssue(TimeInvalid[] invalidLyricTime)
                : base(new Lyric(), new TestIssueTemplate(), invalidLyricTime)
            {
            }
        }

        internal class TestTimeTagIssue : TimeTagIssue
        {
            public TestTimeTagIssue(Dictionary<TimeTagInvalid, TimeTag[]> invalidTimeTags, bool missingStartTimeTag = false, bool missingEndTimeTag = false)
                : base(new Lyric(), new TestIssueTemplate(), invalidTimeTags, missingStartTimeTag, missingEndTimeTag)
            {
            }
        }
    }
}
