﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Reflection;
using osu.Framework.Allocation;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Singers;
using osu.Game.Rulesets.Karaoke.Edit.Components.Menus;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Fonts;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Components.Menus;
using osu.Game.Screens.Edit.Components.TernaryButtons;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class KaraokeHitObjectComposer : HitObjectComposer<KaraokeHitObject>
    {
        private DrawableKaraokeEditorRuleset drawableRuleset;

        [Cached]
        private readonly KaraokeRulesetEditConfigManager editConfigManager;

        [Cached]
        private readonly FontManager fontManager;

        [Cached(typeof(ILyricRubyChangeHandler))]
        private readonly LyricRubyChangeHandler lyricRubyChangeHandler;

        [Cached(typeof(ILyricRomajiChangeHandler))]
        private readonly LyricRomajiChangeHandler lyricRomajiChangeHandler;

        [Cached(Type = typeof(INotePositionInfo))]
        private readonly NotePositionInfo notePositionInfo;

        [Cached(typeof(INotesChangeHandler))]
        private readonly NotesChangeHandler notesChangeHandler;

        [Cached(typeof(ILyricSingerChangeHandler))]
        private readonly LyricSingerChangeHandler lyricSingerChangeHandler;

        [Cached(typeof(ILyricLayoutChangeHandler))]
        private readonly LyricLayoutChangeHandler lyricLayoutChangeHandler;

        [Cached(typeof(ISingersChangeHandler))]
        private readonly SingersChangeHandler singersChangeHandler;

        [Resolved]
        private Editor editor { get; set; }

        public KaraokeHitObjectComposer(Ruleset ruleset)
            : base(ruleset)
        {
            editConfigManager = new KaraokeRulesetEditConfigManager();

            // Duplicated registration because selection handler need to use it.
            AddInternal(fontManager = new FontManager());

            AddInternal(lyricRubyChangeHandler = new LyricRubyChangeHandler());
            AddInternal(lyricRomajiChangeHandler = new LyricRomajiChangeHandler());
            AddInternal(notePositionInfo = new NotePositionInfo());
            AddInternal(notesChangeHandler = new NotesChangeHandler());
            AddInternal(lyricSingerChangeHandler = new LyricSingerChangeHandler());
            AddInternal(lyricLayoutChangeHandler = new LyricLayoutChangeHandler());
            AddInternal(singersChangeHandler = new SingersChangeHandler());
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            CreateMenuBar();
        }

        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public new KaraokePlayfield Playfield => drawableRuleset.Playfield;

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

            // should not affect x position and time if dragging object in note playfield.
            return result.Playfield is EditorNotePlayfield
                ? new SnapResult(screenSpacePosition, null, result.Playfield)
                : result;
        }

        protected override DrawableRuleset<KaraokeHitObject> CreateDrawableRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
        {
            drawableRuleset = new DrawableKaraokeEditorRuleset(ruleset, beatmap, mods);

            // This is the earliest we can cache the scrolling info to ourselves, before masks are added to the hierarchy and inject it
            dependencies.CacheAs(drawableRuleset.ScrollingInfo);

            return drawableRuleset;
        }

        protected override ComposeBlueprintContainer CreateBlueprintContainer()
            => new KaraokeBlueprintContainer(this);

        protected void CreateMenuBar()
        {
            // It's a tricky way to place menu bar in here, will be removed eventually.
            var prop = typeof(Editor).GetField("menuBar", BindingFlags.Instance | BindingFlags.NonPublic);
            if (prop == null)
                return;

            var menuBar = (EditorMenuBar)prop.GetValue(editor);

            Schedule(() =>
            {
                menuBar.Items = new List<MenuItem>(menuBar.Items)
                {
                    new("Config")
                    {
                        Items = new MenuItem[]
                        {
                            new EditorMenuItem("Lyric editor"),
                            new GeneratorConfigMenu("Generator"),
                            new EditorMenuItem("open Karaoke editor", MenuItemType.Standard, () =>
                            {
                                editor.Push(new KaraokeEditor());
                            }),
                        }
                    },
                };
            });
        }

        protected override IReadOnlyList<HitObjectCompositionTool> CompositionTools => Array.Empty<HitObjectCompositionTool>();

        protected override IEnumerable<TernaryButton> CreateTernaryButtons() => Array.Empty<TernaryButton>();
    }
}
