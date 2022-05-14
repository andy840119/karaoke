﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public abstract class TextTagEditModeSection : EditModeSection<TextTagEditMode>
    {
        protected override OverlayColourScheme CreateColourScheme()
        {
            return OverlayColourScheme.Pink;
        }

        protected override Color4 GetColour(OsuColour colours, TextTagEditMode mode, bool active)
        {
            return mode switch
            {
                TextTagEditMode.Generate => active ? colours.Blue : colours.BlueDarker,
                TextTagEditMode.Edit => active ? colours.Red : colours.RedDarker,
                TextTagEditMode.Verify => active ? colours.Yellow : colours.YellowDarker,
                _ => throw new ArgumentOutOfRangeException(nameof(mode))
            };
        }
    }
}
