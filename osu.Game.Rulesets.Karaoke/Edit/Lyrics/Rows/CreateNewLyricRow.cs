﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows
{
    public class CreateNewLyricRow : LyricEditorRow
    {
        [Resolved]
        private ILyricsChangeHandler lyricsChangeHandler { get; set; }

        public CreateNewLyricRow()
            : base(new Lyric { Text = "New lyric" })
        {
        }

        protected override Drawable CreateLyricInfo(Lyric lyric)
        {
            return new Container
            {
                RelativeSizeAxes = Axes.Both,
                Child = new IconButton
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Icon = FontAwesome.Solid.PlusCircle,
                    Size = new Vector2(32),
                    TooltipText = "Click to add new lyric",
                    Action = () =>
                    {
                        lyricsChangeHandler.InsertDefaultToLast();
                    }
                }
            };
        }

        protected override Drawable CreateContent(Lyric lyric)
        {
            return new Container();
        }
    }
}
