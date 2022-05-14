﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    public class NoteSwitchSpecialActionSection : SpecialActionSection<NoteEditModeSpecialAction>
    {
        protected override string SwitchActionTitle => "Special actions";
        protected override string SwitchActionDescription => "Auto-generate, edit or clear the notes.";

        protected override void UpdateActionArea(NoteEditModeSpecialAction action)
        {
            RemoveAll(x => x is NoteAutoGenerateSubsection or NoteClearSubsection);

            switch (action)
            {
                case NoteEditModeSpecialAction.AutoGenerate:
                    Add(new NoteAutoGenerateSubsection());
                    break;

                case NoteEditModeSpecialAction.SyncTime:
                    // todo: implement
                    break;

                case NoteEditModeSpecialAction.Clear:
                    Add(new NoteClearSubsection());
                    break;

                default:
                    return;
            }
        }

        [BackgroundDependencyLoader]
        private void load(IEditNoteModeState editNoteModeState)
        {
            BindTo(editNoteModeState);
        }
    }
}
