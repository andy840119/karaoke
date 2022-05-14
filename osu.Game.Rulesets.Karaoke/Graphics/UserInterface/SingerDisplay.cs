// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterface
{
    public class SingerDisplay : Container, IHasCurrentValue<IReadOnlyList<Singer>>
    {
        public bool DisplayUnrankedText = true;

        public ExpansionMode ExpansionMode = ExpansionMode.ExpandOnHover;

        public Bindable<IReadOnlyList<Singer>> Current
        {
            get => current;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                current.UnbindBindings();
                current.BindTo(value);
            }
        }

        private const int fade_duration = 1000;

        private readonly Bindable<IReadOnlyList<Singer>> current = new();

        private readonly FillFlowContainer<DrawableSinger> iconsContainer;

        public SingerDisplay()
        {
            AutoSizeAxes = Axes.Both;

            Child = new FillFlowContainer
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Children = new Drawable[]
                {
                    iconsContainer = new ReverseChildIDFillFlowContainer<DrawableSinger>
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Horizontal
                    }
                }
            };

            Current.ValueChanged += singers =>
            {
                iconsContainer.Clear();

                foreach (var singer in singers.NewValue)
                {
                    iconsContainer.Add(new DrawableSinger
                    {
                        Singer = singer,
                        Name = "Avatar",
                        Size = new Vector2(32)
                    });
                }

                if (IsLoaded)
                    appearTransform();
            };
        }

        #region Disposal

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            Current.UnbindAll();
        }

        #endregion

        protected override void LoadComplete()
        {
            base.LoadComplete();

            appearTransform();
            iconsContainer.FadeInFromZero(fade_duration, Easing.OutQuint);
        }

        protected override bool OnHover(HoverEvent e)
        {
            expand();
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            contract();
            base.OnHoverLost(e);
        }

        private void appearTransform()
        {
            expand();

            using (iconsContainer.BeginDelayedSequence(1200))
                contract();
        }

        private void expand()
        {
            if (ExpansionMode != ExpansionMode.AlwaysContracted)
                iconsContainer.TransformSpacingTo(new Vector2(5, 0), 500, Easing.OutQuint);
        }

        private void contract()
        {
            if (ExpansionMode != ExpansionMode.AlwaysExpanded)
                iconsContainer.TransformSpacingTo(new Vector2(-25, 0), 500, Easing.OutQuint);
        }

        private class DrawableSinger : DrawableCircleSingerAvatar, IHasCustomTooltip<ISinger>
        {
            public ISinger TooltipContent => Singer;

            public ITooltip<ISinger> GetCustomTooltip()
            {
                return new SingerToolTip();
            }
        }
    }

    public enum ExpansionMode
    {
        /// <summary>
        ///     The <see cref="SingerDisplay" /> will expand only when hovered.
        /// </summary>
        ExpandOnHover,

        /// <summary>
        ///     The <see cref="SingerDisplay" /> will always be expanded.
        /// </summary>
        AlwaysExpanded,

        /// <summary>
        ///     The <see cref="SingerDisplay" /> will always be contracted.
        /// </summary>
        AlwaysContracted
    }
}
