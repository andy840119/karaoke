﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.UserInterfaceV2
{
    public class NoteEditPopover : OsuPopover
    {
        public NoteEditPopover(Note note)
        {
            Children = new Drawable[]
            {
                new FillFlowContainer
                {
                    Width = 200,
                    Direction = FillDirection.Vertical,
                    AutoSizeAxes = Axes.Y,
                    Children = new Drawable[]
                    {
                        new LabelledTextBox
                        {
                            Label = "Text",
                            Description = "The text display on the note.",
                            Current = note.TextBindable
                        },
                        new LabelledTextBox
                        {
                            Label = "Ruby text",
                            Description = "Should placing something like ruby, 拼音 or ふりがな.",
                            Current = note.RubyTextBindable
                        },
                        new LabelledSwitchButton
                        {
                            Label = "Display",
                            Description = "This note will be hidden and not scorable if not display.",
                            Current = note.DisplayBindable,
                        }
                    }
                }
            };
        }
    }
}