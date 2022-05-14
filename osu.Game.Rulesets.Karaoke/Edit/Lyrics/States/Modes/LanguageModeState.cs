// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Languages;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes
{
    public class LanguageModeState : Component, ILanguageModeState
    {
        public Bindable<LanguageEditModeSpecialAction> BindableSpecialAction { get; } = new();

        public IBindable<LanguageEditMode> BindableEditMode => bindableEditMode;
        private readonly Bindable<LanguageEditMode> bindableEditMode = new();

        public void ChangeEditMode(LanguageEditMode mode)
        {
            bindableEditMode.Value = mode;
        }
    }
}
