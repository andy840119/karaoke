﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RomajiTagEditSection : TextTagEditSection<RomajiTag>
    {
        protected override string Title => "Romaji";

        [BackgroundDependencyLoader]
        private void load(LyricCaretState lyricCaretState)
        {
            lyricCaretState.BindableCaretPosition.BindValueChanged(e =>
            {
                Lyric = e.NewValue?.Lyric;

                if (e.OldValue?.Lyric != null)
                {
                    TextTags.UnbindFrom(e.OldValue.Lyric.RomajiTagsBindable);
                }

                if (e.NewValue?.Lyric != null)
                {
                    TextTags.BindTo(e.NewValue.Lyric.RomajiTagsBindable);
                }
            }, true);
        }

        protected override LabelledTextTagTextBox<RomajiTag> CreateLabelledTextTagTextBox(RomajiTag textTag)
            => new LabelledRomajiTagTextBox(textTag);

        protected class LabelledRomajiTagTextBox : LabelledTextTagTextBox<RomajiTag>
        {
            public LabelledRomajiTagTextBox(RomajiTag textTag)
                : base(textTag)
            {
            }

            [BackgroundDependencyLoader]
            private void load(BlueprintSelectionState blueprintSelectionState)
            {
                blueprintSelectionState.SelectedRomajiTags.BindTo(SelectedTextTag);
            }
        }
    }
}
