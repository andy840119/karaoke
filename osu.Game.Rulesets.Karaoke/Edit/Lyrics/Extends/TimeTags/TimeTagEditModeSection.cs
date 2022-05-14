﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components.Description;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagEditModeSection : EditModeSection<TimeTagEditMode>
    {
        [Resolved]
        private ITimeTagModeState timeTagModeState { get; set; }

        internal override void UpdateEditMode(TimeTagEditMode mode)
        {
            timeTagModeState.ChangeEditMode(mode);

            base.UpdateEditMode(mode);
        }

        protected override OverlayColourScheme CreateColourScheme()
        {
            return OverlayColourScheme.Orange;
        }

        protected override TimeTagEditMode DefaultMode()
        {
            return timeTagModeState.EditMode;
        }

        protected override Dictionary<TimeTagEditMode, EditModeSelectionItem> CreateSelections()
        {
            return new()
            {
                {
                    TimeTagEditMode.Create, new EditModeSelectionItem("Adjust", "Create the time-tag or adjust the position.")
                },
                {
                    TimeTagEditMode.Recording, new EditModeSelectionItem("Recording", new DescriptionFormat
                    {
                        Text = "Press [key](set_time_tag_time) at the right time to set current time to time-tag. Press [key](clear_time_tag_time) to clear the time-tag time.",
                        Keys = new Dictionary<string, InputKey>
                        {
                            {
                                "set_time_tag_time", new InputKey
                                {
                                    AdjustableActions = new List<KaraokeEditAction> { KaraokeEditAction.SetTime }
                                }
                            },
                            {
                                "clear_time_tag_time", new InputKey
                                {
                                    AdjustableActions = new List<KaraokeEditAction> { KaraokeEditAction.ClearTime }
                                }
                            }
                        }
                    })
                },
                {
                    TimeTagEditMode.Adjust, new EditModeSelectionItem("Adjust", "Drag to adjust time-tag time precisely.")
                }
            };
        }

        protected override Color4 GetColour(OsuColour colours, TimeTagEditMode mode, bool active)
        {
            return mode switch
            {
                TimeTagEditMode.Create => active ? colours.Blue : colours.BlueDarker,
                TimeTagEditMode.Recording => active ? colours.Red : colours.RedDarker,
                TimeTagEditMode.Adjust => active ? colours.Yellow : colours.YellowDarker,
                _ => throw new ArgumentOutOfRangeException(nameof(mode))
            };
        }
    }
}
