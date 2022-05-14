﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Framework.Platform;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Screens.Settings.Previews;
using osu.Game.Rulesets.Karaoke.Screens.Settings.Previews.Graphics;
using osu.Game.Rulesets.Karaoke.Skinning.Fonts;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings.Sections.Graphics
{
    public class ManageFontSettings : KaraokeSettingsSubsection
    {
        protected override LocalisableString Header => "Font Management";

        public override SettingsSubsectionPreview CreatePreview()
        {
            return new ManageFontPreview();
        }

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            Children = new Drawable[]
            {
                new SettingsButton
                {
                    Text = "Open import text folder",
                    TooltipText = "After open the folder, you can drag the font file to the folder you wants to import",
                    Action = () => storage.GetStorageForDirectory(FontManager.FONT_BASE_PATH).PresentExternally()
                },
                new SettingsButton
                {
                    Text = "Import file",
                    TooltipText = "If some font is placed into folder but not import, press here to try again."
                }
            };
        }
    }
}
