// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Localisation;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage;

public partial class ClassicLyricLayoutPopover : OsuPopover
{
    public ClassicLyricLayoutPopover(ClassicLyricLayout classicLyricLayout)
    {
        Child = new GridContainer
        {
            Height = 500,
            Width = 300,
            ColumnDimensions = new[]
            {
                new Dimension(GridSizeMode.Relative, 300),
                new Dimension(GridSizeMode.AutoSize),
            },
            Content = new[]
            {
                new[]
                {
                    new OsuScrollContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        Children = new[]
                        {
                            new LyricLayoutSection(classicLyricLayout)
                        }
                    },
                    // todo: place the preview area in here.
                }
            }
        };
    }

    public partial class LyricLayoutSection : EditorSection
    {
        protected override LocalisableString Title => "Generic";

        public LyricLayoutSection(ClassicLyricLayout classicLyricLayout)
        {

        }

        private void load(IBeatmapStageElementCategoryChangeHandler<ClassicLyricLayout> changeHandler)
        {
        }
    }
}
