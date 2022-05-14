// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Specialized;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Graphics.Containers
{
    public abstract class OrderRearrangeableListContainer<TModel> : OsuRearrangeableListContainer<TModel>
    {
        public bool DisplayBottomDrawable
        {
            get => displayBottomDrawable;
            set
            {
                if (displayBottomDrawable == value)
                    return;

                displayBottomDrawable = value;

                if (displayBottomDrawable)
                {
                    bottomDrawable = CreateBottomDrawable();
                    if (bottomDrawable == null)
                        return;

                    bottomDrawable.Anchor |= Anchor.y2;
                    bottomDrawable.Origin |= Anchor.y2;

                    // because scroll container only follow list container size, so change the margin to let content bigger.
                    ListContainer.Margin = new MarginPadding { Bottom = bottomDrawable.Height + Spacing.Y };
                    ScrollContainer.Add(bottomDrawable);
                }
                else
                {
                    if (bottomDrawable == null)
                        return;

                    ListContainer.Margin = new MarginPadding();
                    ScrollContainer.Remove(bottomDrawable);
                }
            }
        }

        protected abstract Vector2 Spacing { get; }

        private bool displayBottomDrawable;
        private Drawable bottomDrawable;

        protected OrderRearrangeableListContainer()
        {
            // this collection change event cannot directly register in parent bindable.
            // So register in here.
            Items.CollectionChanged += collectionChanged;
        }

        protected override FillFlowContainer<RearrangeableListItem<TModel>> CreateListFillFlowContainer()
        {
            return base.CreateListFillFlowContainer().With(x => x.Spacing = Spacing);
        }

        protected virtual Drawable CreateBottomDrawable()
        {
            return null;
        }

        private void collectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                // should get the event if user change the position.
                case NotifyCollectionChangedAction.Move:
                    var item = (TModel)e.NewItems[0];
                    int newIndex = e.NewStartingIndex;
                    OnOrderChanged?.Invoke(item, newIndex);
                    break;
            }
        }

        public event Action<TModel, int> OnOrderChanged;
    }
}
