﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;

namespace osu.Game.Rulesets.Karaoke.Graphics.Containers
{
    public class MessageContainer : OsuTextFlowContainer
    {
        [Resolved]
        private OsuColour colours { get; set; }

        public MessageContainer(Action<SpriteText> defaultCreationParameters = null)
            : base(defaultCreationParameters)
        {
        }

        public void AddSuccessParagraph(string message)
        {
            AddIcon(FontAwesome.Solid.Check, icon =>
            {
                icon.Colour = colours.Green;
            });
            AddText($" {message}");
            NewLine();
        }

        public void AddWarningParagraph(string message)
        {
            AddIcon(FontAwesome.Solid.ExclamationTriangle, icon =>
            {
                icon.Colour = colours.Yellow;
                icon.Scale = new osuTK.Vector2(0.9f);
            });
            AddText($" {message}");
            NewLine();
        }

        public void AddAlertParagraph(string message)
        {
            AddIcon(FontAwesome.Solid.TimesCircle, icon =>
            {
                icon.Colour = colours.Red;
            });
            AddText($" {message}");
            NewLine();
        }
    }
}
