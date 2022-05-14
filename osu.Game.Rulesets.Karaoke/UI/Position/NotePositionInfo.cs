﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.UI.Position
{
    public class NotePositionInfo : SkinReloadableDrawable, INotePositionInfo
    {
        public new IBindable<NotePositionCalculator> Position => position;
        public NotePositionCalculator Calculator => Position.Value;
        private const int columns = 9;

        private readonly Bindable<NotePositionCalculator> position = new();

        private readonly IBindable<int> bindableColumns = new Bindable<int>(columns);
        private readonly IBindable<float> bindableColumnHeight = new Bindable<float>(DefaultColumnBackground.COLUMN_HEIGHT);
        private readonly IBindable<float> bindableColumnSpacing = new Bindable<float>(ScrollingNotePlayfield.COLUMN_SPACING);

        public NotePositionInfo()
        {
            bindableColumnHeight.BindValueChanged(_ => updatePositionCalculator());
            bindableColumnSpacing.BindValueChanged(_ => updatePositionCalculator());

            updatePositionCalculator();
        }

        protected override void SkinChanged(ISkinSource skin)
        {
            base.SkinChanged(skin);

            bindableColumnHeight.UnbindBindings();
            bindableColumnSpacing.UnbindBindings();

            var columnHeight = skin.GetConfig<KaraokeSkinConfigurationLookup, float>(new KaraokeSkinConfigurationLookup(columns, LegacyKaraokeSkinConfigurationLookups.ColumnHeight));
            if (columnHeight == null)
                return;

            var columnSpacing = skin.GetConfig<KaraokeSkinConfigurationLookup, float>(new KaraokeSkinConfigurationLookup(columns, LegacyKaraokeSkinConfigurationLookups.ColumnSpacing));
            if (columnSpacing == null)
                return;

            bindableColumnHeight.BindTo(columnHeight);
            bindableColumnSpacing.BindTo(columnSpacing);
        }

        private void updatePositionCalculator()
        {
            position.Value = new NotePositionCalculator(bindableColumns.Value, bindableColumnHeight.Value, bindableColumnSpacing.Value);
        }
    }
}
