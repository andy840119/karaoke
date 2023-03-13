// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage;

public partial class ClassicLyricLayoutPopover : OsuPopover
{
    public ClassicLyricLayoutPopover(ClassicLyricLayout classicLyricLayout)
    {

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
