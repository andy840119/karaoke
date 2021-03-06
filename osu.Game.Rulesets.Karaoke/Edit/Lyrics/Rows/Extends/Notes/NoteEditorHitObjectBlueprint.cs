﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Notes.Components;
using osu.Game.Rulesets.Karaoke.Edit.Notes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.Notes
{
    /// <summary>
    /// Copy from <see cref="NoteSelectionBlueprint"/>
    /// </summary>
    public class NoteEditorHitObjectBlueprint : SelectionBlueprint<Note>
    {
        private readonly IBindable<double> timeRange = new BindableDouble();
        private readonly IBindable<ScrollingDirection> direction = new Bindable<ScrollingDirection>();

        private float scrollLength => Parent.DrawWidth;

        private bool axisInverted => direction.Value == ScrollingDirection.Right;

        [Resolved]
        private NoteManager noteManager { get; set; }

        [Resolved]
        private Playfield playfield { get; set; }

        [Resolved]
        private IScrollingInfo scrollingInfo { get; set; }

        [Resolved]
        private INotePositionInfo notePositionInfo { get; set; }

        private readonly Lyric lyric;

        public NoteEditorHitObjectBlueprint(Lyric lyric, Note note)
            : base(note)
        {
            this.lyric = lyric;

            RelativeSizeAxes = Axes.None;
            AddInternal(new EditBodyPiece
            {
                RelativeSizeAxes = Axes.Both
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            direction.BindTo(scrollingInfo.Direction);
            timeRange.BindTo(scrollingInfo.TimeRange);
        }

        protected override void Update()
        {
            base.Update();

            var anchor = scrollingInfo.Direction.Value == ScrollingDirection.Left ? Anchor.CentreLeft : Anchor.CentreRight;
            Anchor = Origin = anchor;

            Position = Parent.ToLocalSpace(screenSpacePositionAtTime(Item.StartTime)) - AnchorPosition;
            Y += notePositionInfo.Calculator.YPositionAt(Item.Tone);

            Width = lengthAtTime(Item.StartTime, Item.EndTime);
            Height = notePositionInfo.Calculator.ColumnHeight;
        }

        private Vector2 screenSpacePositionAtTime(double time)
        {
            float localPosition = positionAtTime(time, lyric.LyricStartTime);
            localPosition += axisInverted ? scrollLength : 0;
            return Parent.ToScreenSpace(new Vector2(localPosition, Parent.DrawHeight / 2));
        }

        private float positionAtTime(double time, double currentTime)
        {
            float scrollPosition = scrollingInfo.Algorithm.PositionAt(time, currentTime, timeRange.Value, scrollLength);
            return axisInverted ? -scrollPosition : scrollPosition;
        }

        private float lengthAtTime(double startTime, double endTime)
        {
            return scrollingInfo.Algorithm.GetLength(startTime, endTime, timeRange.Value, scrollLength);
        }

        public override MenuItem[] ContextMenuItems => new MenuItem[]
        {
            new OsuMenuItem(Item.Display ? "Hide" : "Show", Item.Display ? MenuItemType.Destructive : MenuItemType.Standard, () => noteManager.ChangeDisplay(Item, !Item.Display)),
            new OsuMenuItem("Split", MenuItemType.Destructive, () => noteManager.SplitNote(Item)),
        };
    }
}
