// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.UI.PlayerSettings;

namespace osu.Game.Rulesets.Karaoke.UI.HUD
{
    public class PracticeOverlay : SettingOverlay
    {
        public PracticeOverlay(IBeatmap beatmap)
        {
            Add(new PracticeSettings(beatmap)
            {
                Expanded =
                {
                    Value = true
                },
                Width = 400
            });
        }

        public override SettingButton CreateToggleButton() => new()
        {
            Name = "Toggle Practice",
            Text = "Practice",
            TooltipText = "Open/Close practice overlay",
            Action = ToggleVisibility
        };
    }
}