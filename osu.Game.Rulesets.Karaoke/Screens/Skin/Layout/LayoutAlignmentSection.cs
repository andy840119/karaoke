﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Layout
{
    internal class LayoutAlignmentSection : LayoutSection
    {
        protected override string Title => "Layout";
        private LabelledEnumDropdown<Anchor> alignmentDropdown;
        private LabelledRealTimeSliderBar<int> horizontalMarginSliderBar;
        private LabelledRealTimeSliderBar<int> verticalMarginSliderBar;

        [BackgroundDependencyLoader]
        private void load(LayoutManager manager)
        {
            Children = new Drawable[]
            {
                alignmentDropdown = new LabelledEnumDropdown<Anchor>
                {
                    Label = "Anchor",
                    Description = "Anchor section"
                },
                horizontalMarginSliderBar = new LabelledRealTimeSliderBar<int>
                {
                    Label = "Horizontal margin",
                    Description = "Horizontal margin section",
                    Current = new BindableNumber<int>
                    {
                        MinValue = 0,
                        MaxValue = 500,
                        Value = 30,
                        Default = 30
                    }
                },
                verticalMarginSliderBar = new LabelledRealTimeSliderBar<int>
                {
                    Label = "Vertical margin",
                    Description = "Vertical margin section",
                    Current = new BindableNumber<int>
                    {
                        MinValue = 0,
                        MaxValue = 500,
                        Value = 30,
                        Default = 30
                    }
                }
            };

            manager.LoadedLayout.BindValueChanged(e =>
            {
                var layout = e.NewValue;
                applyCurrent(alignmentDropdown.Current, layout.Alignment);
                applyCurrent(horizontalMarginSliderBar.Current, layout.HorizontalMargin);
                applyCurrent(verticalMarginSliderBar.Current, layout.VerticalMargin);

                static void applyCurrent<T>(Bindable<T> bindable, T value)
                    => bindable.Value = bindable.Default = value;
            }, true);

            alignmentDropdown.Current.BindValueChanged(x => manager.ApplyCurrentLayoutChange(l => l.Alignment = x.NewValue));
            horizontalMarginSliderBar.Current.BindValueChanged(x => manager.ApplyCurrentLayoutChange(l => l.HorizontalMargin = x.NewValue));
            verticalMarginSliderBar.Current.BindValueChanged(x => manager.ApplyCurrentLayoutChange(l => l.VerticalMargin = x.NewValue));
        }
    }
}
