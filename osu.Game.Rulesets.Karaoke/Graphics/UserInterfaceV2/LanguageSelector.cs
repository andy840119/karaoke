﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2
{
    public class LanguageSelector : CompositeDrawable
    {
        public Bindable<CultureInfo> Current
        {
            get => current.Current;
            set => current.Current = value;
        }

        private readonly BindableWithCurrent<CultureInfo> current = new();

        public LanguageSelector()
        {
            var languages = new BindableList<CultureInfo>(CultureInfoUtils.GetAvailableLanguages());

            LanguageSelectionSearchTextBox filter;
            RearrangeableLanguageListContainer languageList;
            InternalChild = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.Absolute, 40),
                    new Dimension()
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        filter = new LanguageSelectionSearchTextBox
                        {
                            RelativeSizeAxes = Axes.X
                        }
                    },
                    new Drawable[]
                    {
                        languageList = new RearrangeableLanguageListContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            RequestSelection = item =>
                            {
                                Current.Value = item;
                            },
                            Items = { BindTarget = languages }
                        }
                    }
                }
            };

            filter.Current.BindValueChanged(e => languageList.Filter(e.NewValue));
            Current.BindValueChanged(e => languageList.SelectedSet.Value = e.NewValue);
        }

        private class LanguageSelectionSearchTextBox : SearchTextBox
        {
            protected override Color4 SelectionColour => Color4.Gray;

            public LanguageSelectionSearchTextBox()
            {
                PlaceholderText = @"type in keywords...";
            }
        }

        private class RearrangeableLanguageListContainer : RearrangeableTextFlowListContainer<CultureInfo>
        {
            protected override DrawableTextListItem CreateDrawable(CultureInfo item)
            {
                return new DrawableLanguageListItem(item);
            }

            private class DrawableLanguageListItem : DrawableTextListItem
            {
                public override IEnumerable<string> FilterTerms => new[]
                {
                    Model.Name,
                    Model.DisplayName,
                    Model.EnglishName,
                    Model.NativeName
                };

                public DrawableLanguageListItem(CultureInfo item)
                    : base(item)
                {
                    Padding = new MarginPadding { Left = 5 };
                }

                protected override void CreateDisplayContent(OsuTextFlowContainer textFlowContainer, CultureInfo model)
                {
                    textFlowContainer.AddText(CultureInfoUtils.GetLanguageDisplayText(model));
                }
            }
        }
    }
}
