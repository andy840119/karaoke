﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Layout
{
    public class LayoutManager : Component
    {
        public readonly BindableList<LyricLayout> Layouts = new();

        public readonly Bindable<LyricLayout> LoadedLayout = new();

        public readonly Bindable<LyricLayout> EditLayout = new();

        public readonly IDictionary<int, string> PreviewFontSelections = new Dictionary<int, string>();

        public readonly Bindable<Lyric> PreviewLyric = new();

        public readonly Bindable<DisplayRatio> PreviewScreenRatio = new();

        public readonly Bindable<int[]> PreviewSingers = new();

        [Resolved]
        private ISkinSource source { get; set; }

        public void ApplyCurrentLayoutChange(Action<LyricLayout> action)
        {
            action?.Invoke(EditLayout.Value);
            EditLayout.TriggerChange();
        }

        public void ChangeCurrentLayout(LyricLayout layout)
        {
            LoadedLayout.Value = layout;
            EditLayout.Value = layout;
        }

        public void ChangePreviewSinger(int[] singers)
        {
            if (singers != null)
                PreviewSingers.Value = singers;
        }

        public void UpdateLayoutName(LyricLayout layout, string newValue)
        {
            throw new NotImplementedException();
        }

        public void AddLayout(LyricLayout layout)
        {
            throw new NotImplementedException();
        }

        public bool IsLayoutModified(LyricLayout layout)
        {
            throw new NotImplementedException();
        }

        public void RemoveLayout(LyricLayout layout)
        {
            throw new NotImplementedException();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            var layoutLookups = source.GetConfig<KaraokeIndexLookup, IDictionary<int, string>>(KaraokeIndexLookup.Layout)?.Value;

            foreach (var layoutLookup in layoutLookups)
            {
                var lookup = new KaraokeSkinLookup(ElementType.LyricLayout, layoutLookup.Key);
                var layout = source.GetConfig<KaraokeSkinLookup, LyricLayout>(lookup)?.Value;
                if (layout != null)
                    Layouts.Add(layout);
            }

            LoadedLayout.Value = Layouts.FirstOrDefault();
            EditLayout.Value = Layouts.FirstOrDefault();

            var skinLookups = source.GetConfig<KaraokeIndexLookup, IDictionary<int, string>>(KaraokeIndexLookup.Style)?.Value;

            if (skinLookups == null)
                return;

            foreach ((int key, string value) in skinLookups) PreviewFontSelections.Add(key, value);
        }
    }

    public struct DisplayRatio
    {
        public float Width { get; set; }

        public float Height { get; set; }

        public bool IsValid()
        {
            return Width > 0 && Height > 0;
        }
    }
}
