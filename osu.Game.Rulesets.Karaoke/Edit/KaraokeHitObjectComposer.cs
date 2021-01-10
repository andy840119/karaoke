﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Reflection;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Beatmaps;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Components.Menu;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Components.Menus;
using osu.Game.Screens.Edit.Components.TernaryButtons;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class KaraokeHitObjectComposer : HitObjectComposer<KaraokeHitObject>
    {
        private DrawableKaraokeEditRuleset drawableRuleset;
        private readonly LyricEditor lyricEditor;
        private readonly Bindable<EditMode> bindableEditMode = new Bindable<EditMode>();
        private readonly Bindable<Mode> bindableLyricEditorMode = new Bindable<Mode>();
        private readonly Bindable<LyricFastEditMode> bindableLyricEditorFastEditMode = new Bindable<LyricFastEditMode>();

        [Cached(Type = typeof(IPositionCalculator))]
        private readonly PositionCalculator positionCalculator;

        [Cached]
        private readonly KaraokeRulesetEditConfigManager editConfigManager;

        [Cached]
        private readonly KaraokeRulesetEditGeneratorConfigManager generatorConfigManager;

        [Resolved]
        private Editor editor { get; set; }

        public KaraokeHitObjectComposer(Ruleset ruleset)
            : base(ruleset)
        {
            // Duplicated registration because selection handler need to use it.
            positionCalculator = new PositionCalculator(9);
            editConfigManager = new KaraokeRulesetEditConfigManager();
            generatorConfigManager = new KaraokeRulesetEditGeneratorConfigManager();

            LayerBelowRuleset.Add(lyricEditor = new LyricEditor
            {
                RelativeSizeAxes = Axes.Both
            });

            bindableEditMode.BindValueChanged(e =>
            {
                if (e.NewValue == EditMode.LyricEditor)
                    lyricEditor.Show();
                else
                    lyricEditor.Hide();
            });
            bindableLyricEditorMode.BindValueChanged(e =>
            {
                lyricEditor.Mode = e.NewValue;
            });
            bindableLyricEditorFastEditMode.BindValueChanged(e =>
            {
                lyricEditor.LyricFastEditMode = e.NewValue;
            });

            editConfigManager.BindWith(KaraokeRulesetEditSetting.EditMode, bindableEditMode);
            editConfigManager.BindWith(KaraokeRulesetEditSetting.LyricEditorMode, bindableLyricEditorMode);
            editConfigManager.BindWith(KaraokeRulesetEditSetting.LyricEditorFastEditMode, bindableLyricEditorFastEditMode);
            
        }

        public new KaraokePlayfield Playfield => drawableRuleset.Playfield;

        public IScrollingInfo ScrollingInfo => drawableRuleset.ScrollingInfo;

        protected override Playfield PlayfieldAtScreenSpacePosition(Vector2 screenSpacePosition)
        {
            // Only note and lyric playfield can interact with mouse input.
            if (Playfield.NotePlayfield.ReceivePositionalInputAt(screenSpacePosition))
                return Playfield.NotePlayfield;
            if (Playfield.LyricPlayfield.ReceivePositionalInputAt(screenSpacePosition))
                return Playfield.LyricPlayfield;

            return null;
        }

        public override SnapResult SnapScreenSpacePositionToValidTime(Vector2 screenSpacePosition)
        {
            var result = base.SnapScreenSpacePositionToValidTime(screenSpacePosition);

            if (result.Playfield is NotePlayfield)
            {
                // Apply Y value because it's disappeared.
                result.ScreenSpacePosition.Y = screenSpacePosition.Y;
                // then disable time change by moving x
                result.Time = null;
            }

            return result;
        }

        protected override DrawableRuleset<KaraokeHitObject> CreateDrawableRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            => drawableRuleset = new DrawableKaraokeEditRuleset(ruleset, beatmap, mods);

        protected override ComposeBlueprintContainer CreateBlueprintContainer()
            => new KaraokeBlueprintContainer(this);

        protected void CreateMenuBar()
        {
            // It's a reicky way to place menu bar in here, will be removed eventually.
            var prop = typeof(Editor).GetField("menuBar", BindingFlags.Instance | BindingFlags.NonPublic);
            if (prop == null)
                return;
            var menuBar = (EditorMenuBar)prop.GetValue(editor);

            Schedule(() =>
            {
                menuBar.Items = new[]
                {
                    new MenuItem("File")
                    {
                        Items = new[]
                        {
                            new EditorMenuItem("Import from text"),
                            new EditorMenuItem("Import from .lrc file"),
                            new EditorMenuItemSpacer(),
                            new EditorMenuItem("Export to .lrc"),
                            new EditorMenuItem("Export to text"),
                        }
                    },
                    new MenuItem("View")
                    {
                        Items = new MenuItem[]
                        {
                            new EditModeMenu(editConfigManager, "Edit mode"),
                            new EditorMenuItemSpacer(),
                            new LyricEditorEditModeMenu(editConfigManager, "Lyric editor mode"),
                            new LyricEditorLeftSideModeMenu(editConfigManager, "Lyric editor mode"),
                            new LyricEditorTextSizeMenu(editConfigManager, "Text size"),
                        }
                    },
                    new MenuItem("Tools")
                    {
                        Items = new MenuItem[]
                        {
                            new EditorMenuItem("Singer manager"),
                            new EditorMenuItem("Translate manager"),
                            new EditorMenuItem("Layout manager"),
                            new EditorMenuItem("Style manager"),
                        }
                    },
                    new MenuItem("Options")
                    {
                        Items = new MenuItem[]
                        {
                            new EditorMenuItem("Lyric editor"),
                            new GeneratorConfigMenu("Generator"),
                        }
                    }
                };
            });
        }

        protected override IReadOnlyList<HitObjectCompositionTool> CompositionTools => Array.Empty<HitObjectCompositionTool>();

        private readonly Bindable<TernaryState> displayRubyToggle = new Bindable<TernaryState>();
        private readonly Bindable<TernaryState> displayRomajiToggle = new Bindable<TernaryState>();
        private readonly Bindable<TernaryState> displayTranslateToggle = new Bindable<TernaryState>();

        protected override IEnumerable<TernaryButton> CreateTernaryButtons()
            => new[]
            {
                new TernaryButton(displayRubyToggle, "Ruby", () => new SpriteIcon { Icon = FontAwesome.Solid.Ruler }),
                new TernaryButton(displayRomajiToggle, "Romaji", () => new SpriteIcon { Icon = FontAwesome.Solid.Ruler }),
                new TernaryButton(displayTranslateToggle, "Translate", () => new SpriteIcon { Icon = FontAwesome.Solid.Ruler }),
            };

        [BackgroundDependencyLoader]
        private void load()
        {
            var karaokeSessionStatics = drawableRuleset.Session;
            displayRubyToggle.Value = karaokeSessionStatics.Get<bool>(KaraokeRulesetSession.DisplayRuby) ? TernaryState.True : TernaryState.False;
            displayRomajiToggle.Value = karaokeSessionStatics.Get<bool>(KaraokeRulesetSession.DisplayRomaji) ? TernaryState.True : TernaryState.False;
            displayTranslateToggle.Value = karaokeSessionStatics.Get<bool>(KaraokeRulesetSession.UseTranslate) ? TernaryState.True : TernaryState.False;
            displayRubyToggle.BindValueChanged(x => { karaokeSessionStatics.GetBindable<bool>(KaraokeRulesetSession.DisplayRuby).Value = x.NewValue == TernaryState.True; });
            displayRomajiToggle.BindValueChanged(x => { karaokeSessionStatics.GetBindable<bool>(KaraokeRulesetSession.DisplayRomaji).Value = x.NewValue == TernaryState.True; });
            displayTranslateToggle.BindValueChanged(x => { karaokeSessionStatics.GetBindable<bool>(KaraokeRulesetSession.UseTranslate).Value = x.NewValue == TernaryState.True; });

            CreateMenuBar();
        }
    }
}
