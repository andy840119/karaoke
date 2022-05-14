// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.Singers.Rows.Components;
using osu.Game.Screens.Edit.Compose.Components.Timeline;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Rows
{
    public class DefaultLyricPlacementColumn : LyricPlacementColumn
    {
        public static Singer DefaultSinger { get; } = new(0) { Name = "Default" };
        protected const int LEFT_MARGIN = 22;

        // should add extra width because this component is not draggable, which will have extra spacing.
        protected override float SingerInfoSize => INFO_SIZE + LEFT_MARGIN;

        [Resolved]
        private ISingerScreenScrollingInfoProvider scrollingInfoProvider { get; set; }

        public DefaultLyricPlacementColumn()
            : base(DefaultSinger)
        {
        }

        // todo : might display song info?
        protected override Drawable CreateSingerInfo(Singer singer)
        {
            return new Container
            {
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4Extensions.FromHex("333")
                    },
                    new Container<TimelineButton>
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        RelativeSizeAxes = Axes.Y,
                        AutoSizeAxes = Axes.X,
                        Masking = true,
                        Children = new[]
                        {
                            new TimelineButton
                            {
                                RelativeSizeAxes = Axes.Y,
                                Height = 0.5f,
                                Icon = FontAwesome.Solid.SearchPlus,
                                Action = () => changeZoom(1)
                            },
                            new TimelineButton
                            {
                                Anchor = Anchor.BottomLeft,
                                Origin = Anchor.BottomLeft,
                                RelativeSizeAxes = Axes.Y,
                                Height = 0.5f,
                                Icon = FontAwesome.Solid.SearchMinus,
                                Action = () => changeZoom(-1)
                            }
                        }
                    }
                }
            };
        }

        protected override Drawable CreateTimeLinePart(Singer singer)
        {
            return new SingerLyricEditor(singer);
        }

        private void changeZoom(float change)
        {
            scrollingInfoProvider.BindableZoom.Value += change;
        }
    }
}
