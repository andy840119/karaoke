﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public abstract class LyricImporterStepScreenWithLyricEditor : LyricImporterStepScreenWithTopNavigation
    {
        public LyricEditorMode LyricEditorMode
            => lyricEditor.Mode;

        [Cached(typeof(ILockChangeHandler))]
        private readonly LockChangeHandler lockChangeHandler;

        // it's a tricky way to let navigation bar able to get the lyric state.
        // not a good solution, but have no better way now.
        [Cached(typeof(ILyricEditorState))]
        private ImportLyricEditor lyricEditor { get; set; }

        protected LyricImporterStepScreenWithLyricEditor()
        {
            AddInternal(lockChangeHandler = new LockChangeHandler());
        }

        public virtual void SwitchLyricEditorMode(LyricEditorMode mode)
        {
            lyricEditor.SwitchMode(mode);
        }

        protected override Drawable CreateContent()
        {
            return lyricEditor = new ImportLyricEditor
            {
                RelativeSizeAxes = Axes.Both
            };
        }

        protected void PrepareAutoGenerate()
        {
            lyricEditor.PrepareAutoGenerate();
        }

        private class ImportLyricEditor : LyricEditor
        {
            [Resolved]
            private LyricImporterSubScreenStack screenStack { get; set; }

            private ILyricSelectionState lyricSelectionState { get; set; }
            private ILanguageModeState languageModeState { get; set; }
            private IEditRubyModeState editRubyModeState { get; set; }
            private IEditRomajiModeState editRomajiModeState { get; set; }

            public void PrepareAutoGenerate()
            {
                // then open the selecting mode and select all lyrics.
                lyricSelectionState.StartSelecting();
                lyricSelectionState.SelectAll();

                // for some mode, we need to switch to generate section.
                languageModeState.ChangeEditMode(LanguageEditMode.Generate);
                editRubyModeState.ChangeEditMode(TextTagEditMode.Generate);
                editRomajiModeState.ChangeEditMode(TextTagEditMode.Generate);
            }

            public override void NavigateToFix(LyricEditorMode mode)
            {
                switch (mode)
                {
                    case LyricEditorMode.Typing:
                        screenStack.Pop(LyricImporterStep.EditLyric);
                        break;

                    case LyricEditorMode.Language:
                        screenStack.Pop(LyricImporterStep.AssignLanguage);
                        break;

                    case LyricEditorMode.EditTimeTag:
                        screenStack.Pop(LyricImporterStep.GenerateTimeTag);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode));
                }
            }

            protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            {
                var dependencies = base.CreateChildDependencies(parent);
                lyricSelectionState = dependencies.Get<ILyricSelectionState>();
                languageModeState = dependencies.Get<ILanguageModeState>();
                editRubyModeState = dependencies.Get<IEditRubyModeState>();
                editRomajiModeState = dependencies.Get<IEditRomajiModeState>();
                return dependencies;
            }
        }
    }
}
