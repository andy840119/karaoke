﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables
{
    /// <summary>
    ///     Visualises a <see cref="BarLine" />. Although this derives DrawableKaraokeHitObject,
    ///     this does not handle input/sound like a normal hit object.
    /// </summary>
    public class DrawableBarLine : DrawableKaraokeScrollingHitObject<BarLine>
    {
        /// <summary>
        ///     Height of major bar line triangles.
        /// </summary>
        private const float triangle_width = 12;

        /// <summary>
        ///     Offset of the major bar line triangles from the sides of the bar line.
        /// </summary>
        private const float triangle_offset = 9;

        private readonly Bindable<bool> major = new();

        /// <summary>
        ///     The visual line tracker.
        /// </summary>
        private Box line;

        /// <summary>
        ///     Container with triangles. Only visible for major lines.
        /// </summary>
        private Container triangleContainer;

        public DrawableBarLine()
            : this(null)
        {
        }

        public DrawableBarLine([CanBeNull] BarLine barLine)
            : base(barLine)
        {
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            major.BindValueChanged(updateMajor, true);
        }

        protected override void OnApply()
        {
            base.OnApply();
            major.BindTo(HitObject.MajorBindable);
        }

        protected override void OnFree()
        {
            base.OnFree();
            major.UnbindFrom(HitObject.MajorBindable);
        }

        protected override void UpdateInitialTransforms()
        {
        }

        protected override void UpdateStartTimeStateTransforms()
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Y;
            Width = 2f;

            AddRangeInternal(new Drawable[]
            {
                line = new Box
                {
                    Name = "Bar line",
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Color4(255, 204, 33, 255)
                },
                triangleContainer = new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Children = new[]
                    {
                        new EquilateralTriangle
                        {
                            Name = "Up triangle",
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(triangle_width),
                            Y = -triangle_offset,
                            Rotation = 180
                        },
                        new EquilateralTriangle
                        {
                            Name = "Down triangle",
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(triangle_width),
                            Y = triangle_offset,
                            Rotation = 0
                        }
                    }
                }
            });
        }

        private void updateMajor(ValueChangedEvent<bool> major)
        {
            line.Alpha = major.NewValue ? 1f : 0.75f;
            triangleContainer.Alpha = major.NewValue ? 1 : 0;
        }
    }
}
