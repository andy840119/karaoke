﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components
{
    public abstract class SelectLyricButton : OsuButton
    {
        public Func<Dictionary<Lyric, string>> StartSelecting { get; set; }

        protected virtual string StandardText => "Select lyric";

        protected virtual string SelectingText => "Cancel selecting";
        private IBindable<bool> selecting;

        [Resolved]
        private ILyricSelectionState lyricSelectionState { get; set; }

        protected SelectLyricButton()
        {
            RelativeSizeAxes = Axes.X;
            Content.CornerRadius = 15;
        }

        protected virtual void StartSelectingLyrics()
        {
            // update disabled lyrics list.
            var disableLyrics = StartSelecting?.Invoke();
            lyricSelectionState.UpdateDisableLyricList(disableLyrics);

            // then start selecting.
            lyricSelectionState.StartSelecting();
        }

        protected virtual void EndSelectingLyrics()
        {
            lyricSelectionState.EndSelecting(LyricEditorSelectingAction.Cancel);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            selecting = lyricSelectionState.Selecting.GetBoundCopy();
            selecting.BindValueChanged(e =>
            {
                bool isSelecting = e.NewValue;
                BackgroundColour = isSelecting ? colours.Blue : colours.Purple;
                Text = isSelecting ? SelectingText : StandardText;
            }, true);

            Action = () =>
            {
                if (!selecting.Value)
                    StartSelectingLyrics();
                else
                    EndSelectingLyrics();
            };
        }
    }
}
