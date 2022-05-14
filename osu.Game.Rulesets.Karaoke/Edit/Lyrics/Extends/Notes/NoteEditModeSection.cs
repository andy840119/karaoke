﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    public class NoteEditModeSection : EditModeSection<NoteEditMode>
    {
        [Resolved]
        private IEditNoteModeState editNoteModeState { get; set; }

        internal override void UpdateEditMode(NoteEditMode mode)
        {
            editNoteModeState.ChangeEditMode(mode);

            base.UpdateEditMode(mode);
        }

        protected override OverlayColourScheme CreateColourScheme()
        {
            return OverlayColourScheme.Blue;
        }

        protected override NoteEditMode DefaultMode()
        {
            return editNoteModeState.EditMode;
        }

        protected override Dictionary<NoteEditMode, EditModeSelectionItem> CreateSelections()
        {
            return new()
            {
                {
                    NoteEditMode.Generate, new EditModeSelectionItem("Generate", "Using time-tag to create default notes.")
                },
                {
                    NoteEditMode.Edit, new EditModeSelectionItem("Edit", "Batch edit note property in here.")
                },
                {
                    NoteEditMode.Verify, new EditModeSelectionItem("Verify", "Check invalid notes in here.")
                }
            };
        }

        protected override Color4 GetColour(OsuColour colours, NoteEditMode mode, bool active)
        {
            return mode switch
            {
                NoteEditMode.Generate => active ? colours.Blue : colours.BlueDarker,
                NoteEditMode.Edit => active ? colours.Red : colours.RedDarker,
                NoteEditMode.Verify => active ? colours.Yellow : colours.YellowDarker,
                _ => throw new ArgumentOutOfRangeException(nameof(mode))
            };
        }
    }
}
