// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Blueprints;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components
{
    public class TimeTagBlueprintContainer : ExtendBlueprintContainer<TimeTag>
    {
        protected readonly Lyric Lyric;

        [UsedImplicitly]
        private readonly BindableList<TimeTag> timeTags;

        [Resolved]
        private ILyricCaretState lyricCaretState { get; set; }

        public TimeTagBlueprintContainer(Lyric lyric)
        {
            Lyric = lyric;
            timeTags = lyric.TimeTagsBindable.GetBoundCopy();
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            lyricCaretState.MoveCaretToTargetPosition(Lyric);
            return base.OnMouseDown(e);
        }

        protected override SelectionHandler<TimeTag> CreateSelectionHandler()
        {
            return new TimeTagSelectionHandler();
        }

        protected override SelectionBlueprint<TimeTag> CreateBlueprintFor(TimeTag item)
        {
            return new TimeTagSelectionBlueprint(item);
        }

        [BackgroundDependencyLoader]
        private void load(ITimeTagModeState timeTagModeState)
        {
            // Add time tag into blueprint container
            RegisterBindable(timeTags);
        }

        protected class TimeTagSelectionHandler : ExtendSelectionHandler<TimeTag>
        {
            // for now we always allow movement. snapping is provided by the Timeline's "distance" snap implementation
            public override bool HandleMovement(MoveSelectionEvent<TimeTag> moveEvent)
            {
                return true;
            }

            protected override void DeleteItems(IEnumerable<TimeTag> items)
            {
                // todo : delete time-tag
            }

            [BackgroundDependencyLoader]
            private void load(ITimeTagModeState timeTagModeState)
            {
                SelectedItems.BindTo(timeTagModeState.SelectedItems);
            }
        }
    }
}
