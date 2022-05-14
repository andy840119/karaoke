﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Components.Menus;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Screens.Edit.Components.Menus;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    [TestFixture]
    public class TestSceneEditorMenuBar : OsuTestScene
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            var lyricEditorConfig = new KaraokeRulesetLyricEditorConfigManager();
            Add(new Container
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                RelativeSizeAxes = Axes.X,
                Height = 50,
                Y = 50,
                Child = new EditorMenuBar
                {
                    RelativeSizeAxes = Axes.Both,
                    Items = new[]
                    {
                        new MenuItem("File")
                        {
                            Items = new MenuItem[]
                            {
                                new ImportLyricMenu(null, "Import from text", null),
                                new ImportLyricMenu(null, "Import from .lrc file", null),
                                new EditorMenuItemSpacer(),
                                new EditorMenuItem("Export to .lrc", MenuItemType.Standard, () => { }),
                                new EditorMenuItem("Export to text", MenuItemType.Standard, () => { }),
                                new EditorMenuItem("Export to json", MenuItemType.Destructive, () => { })
                            }
                        },
                        new LyricEditorModeMenu(new Bindable<LyricEditorMode>(), "Mode"),
                        new MenuItem("View")
                        {
                            Items = new MenuItem[]
                            {
                                new LyricEditorTextSizeMenu(lyricEditorConfig, "Text size"),
                                new AutoFocusToEditLyricMenu(lyricEditorConfig, "Auto focus to edit lyric")
                            }
                        },
                        new MenuItem("Config")
                        {
                            Items = new MenuItem[]
                            {
                                new EditorMenuItem("Lyric editor"),
                                new GeneratorConfigMenu("Auto-generator"),
                                new LockStateMenu(lyricEditorConfig, "Lock")
                            }
                        },
                        new MenuItem("Tools")
                        {
                            Items = new MenuItem[]
                            {
                                new KaraokeSkinEditorMenu(null, null, "Skin editor")
                            }
                        }
                    }
                }
            });
        }
    }
}
