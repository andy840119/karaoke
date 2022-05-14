// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes
{
    public class EditRomajiModeState : Component, IEditRomajiModeState
    {
        public IBindable<TextTagEditMode> BindableEditMode => bindableEditMode;

        public BindableList<RomajiTag> SelectedItems { get; } = new();
        private readonly Bindable<TextTagEditMode> bindableEditMode = new();

        public void ChangeEditMode(TextTagEditMode mode)
        {
            bindableEditMode.Value = mode;
        }
    }
}
