// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Stages.Classic;

public partial class TestSceneClassicLyricLayoutPopover : OsuTestScene
{
    private ClassicStageInfo stageInfo = null!;
    private BeatmapStageElementCategoryChangeHandler<ClassicLyricLayout, Lyric> changeHandler = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        stageInfo = new ClassicStageInfo
        {
            LyricLayoutDefinition = new ClassicLyricLayoutDefinition
            {
                LineHeight = 100,
            },
            LyricLayoutCategory = new ClassicLyricLayoutCategory
            {
                AvailableElements =
                {
                    new ClassicLyricLayout(1)
                    {
                        Name = "Layout 1",
                        Alignment = ClassicLyricLayoutAlignment.Left,
                        HorizontalMargin = 10,
                        Line = 0
                    },
                    new ClassicLyricLayout(1)
                    {
                        Name = "Layout 2",
                        Alignment = ClassicLyricLayoutAlignment.Center,
                        HorizontalMargin = 10,
                        Line = 0
                    },
                    new ClassicLyricLayout(1)
                    {
                        Name = "Layout 3",
                        Alignment = ClassicLyricLayoutAlignment.Right,
                        HorizontalMargin = 10,
                        Line = 0
                    },
                    new ClassicLyricLayout(1)
                    {
                        Name = "Layout 4",
                        Alignment = ClassicLyricLayoutAlignment.Right,
                        HorizontalMargin = 10,
                        Line = 1
                    }
                }
            }
        };

        var beatmap = new KaraokeBeatmap
        {
            BeatmapInfo =
            {
                Ruleset = new KaraokeRuleset().RulesetInfo,
            },
            StageInfos = new List<StageInfo>
            {
                stageInfo
            }
        };
        var editorBeatmap = new EditorBeatmap(beatmap);
        changeHandler = new BeatmapStageElementCategoryChangeHandler<ClassicLyricLayout, Lyric>(x => x.OfType<ClassicStageInfo>().First().LyricLayoutCategory);
        Dependencies.Cache(editorBeatmap);
        Dependencies.CacheAs<IBeatmapStageElementCategoryChangeHandler<ClassicLyricLayout>>(changeHandler);
    }

    [Test]
    public void TestPopover()
    {
        var allAvailableLayouts = stageInfo.LyricLayoutCategory.AvailableElements;

        foreach (var layout in allAvailableLayouts)
        {
            AddLabel($"Layout: {layout.Name}");
            AddStep("Show dialog", () =>
            {
                var popover = new ClassicLyricLayoutPopover(layout);

                Schedule(() =>
                {
                    Child = popover;
                    Child?.Show();
                });
            });
        }
    }
}
