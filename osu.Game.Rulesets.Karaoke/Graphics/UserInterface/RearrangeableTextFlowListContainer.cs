// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterface
{
    /// <summary>
    ///     Implement most feature for searchable text container.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class RearrangeableTextFlowListContainer<TModel> : OsuRearrangeableListContainer<TModel>
    {
        public readonly Bindable<TModel> SelectedSet = new();

        public Action<TModel> RequestSelection;

        private SearchContainer<RearrangeableListItem<TModel>> searchContainer;

        public void Filter(string text)
        {
            searchContainer.SearchTerm = text;
        }

        protected sealed override FillFlowContainer<RearrangeableListItem<TModel>> CreateListFillFlowContainer()
        {
            return searchContainer = new SearchContainer<RearrangeableListItem<TModel>>
            {
                Spacing = new Vector2(0, 3),
                LayoutDuration = 200,
                LayoutEasing = Easing.OutQuint
            };
        }

        protected sealed override OsuRearrangeableListItem<TModel> CreateOsuDrawable(TModel item)
        {
            return CreateDrawable(item).With(d =>
            {
                d.SelectedSet.BindTarget = SelectedSet;
                d.RequestSelection = set => RequestSelection?.Invoke(set);
            });
        }

        protected new virtual DrawableTextListItem CreateDrawable(TModel item)
        {
            return new(item);
        }

        public class DrawableTextListItem : OsuRearrangeableListItem<TModel>, IFilterable
        {
            public readonly Bindable<TModel> SelectedSet = new();

            public virtual IEnumerable<string> FilterTerms => new[]
            {
                Model.ToString()
            };

            public Action<TModel> RequestSelection;

            public bool MatchingFilter
            {
                get => matchingFilter;
                set
                {
                    if (matchingFilter == value)
                        return;

                    matchingFilter = value;
                    updateFilter();
                }
            }

            public bool FilteringActive { get; set; }

            private TextFlowContainer text;

            private Color4 selectedColour;

            private bool matchingFilter = true;

            public DrawableTextListItem(TModel item)
                : base(item)
            {
                Padding = new MarginPadding { Left = 5 };
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                SelectedSet.BindValueChanged(set =>
                {
                    bool oldValueMatched = EqualityComparer<TModel>.Default.Equals(set.OldValue, Model);
                    bool newValueMatched = EqualityComparer<TModel>.Default.Equals(set.NewValue, Model);
                    if (!oldValueMatched && !newValueMatched)
                        return;

                    text.FadeColour(newValueMatched ? selectedColour : Color4.White, FADE_DURATION);
                }, true);
            }

            protected sealed override Drawable CreateContent()
            {
                return text = new OsuTextFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y
                }.With(x => CreateDisplayContent(x, Model));
            }

            protected override bool OnClick(ClickEvent e)
            {
                RequestSelection?.Invoke(Model);
                return true;
            }

            protected virtual void CreateDisplayContent(OsuTextFlowContainer textFlowContainer, TModel model)
            {
                textFlowContainer.AddText(model.ToString());
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                selectedColour = colours.Yellow;
                HandleColour = colours.Gray5;
            }

            private void updateFilter()
            {
                this.FadeTo(MatchingFilter ? 1 : 0, 200);
            }
        }
    }
}
