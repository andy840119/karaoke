﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Extensions;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.RecordingTimeTags
{
    [Cached]
    public class RecordingTimeTagEditor : TimeTagEditorScrollContainer
    {
        public const float TIMELINE_HEIGHT = 20;

        private readonly CentreMarker centreMarker;

        /// <summary>
        ///     The timeline's scroll position in the last frame.
        /// </summary>
        private float lastScrollPosition;

        /// <summary>
        ///     The track time in the last frame.
        /// </summary>
        private double lastTrackTime;

        /// <summary>
        ///     Whether the user is currently dragging the timeline.
        /// </summary>
        private bool handlingDragInput;

        /// <summary>
        ///     Whether the track was playing before a user drag event.
        /// </summary>
        private bool trackWasPlaying;

        private OsuSpriteText trackTimer;

        [Resolved]
        private EditorClock editorClock { get; set; }

        public RecordingTimeTagEditor(Lyric lyric)
            : base(lyric)
        {
            // We don't want the centre marker to scroll
            AddInternal(centreMarker = new CentreMarker());
        }

        protected override void PostProcessContent(Container content)
        {
            content.Height = TIMELINE_HEIGHT;
            content.Y = 10;
            content.AddRange(new[]
            {
                centreMarker.CreateProxy(),
                new RecordingTimeTagPart(HitObject)
            });
        }

        protected override void Update()
        {
            base.Update();

            // The extrema of track time should be positioned at the centre of the container when scrolled to the start or end
            Content.Margin = new MarginPadding { Horizontal = DrawWidth / 2 };

            trackTimer.Text = editorClock.CurrentTime.ToEditorFormattedString();

            // This needs to happen after transforms are updated, but before the scroll position is updated in base.UpdateAfterChildren
            if (editorClock.IsRunning)
                scrollToTrackTime();
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            if (handlingDragInput)
                seekTrackToCurrent();
            else if (!editorClock.IsRunning)
            {
                // The track isn't running. There are three cases we have to be wary of:
                // 1) The user flick-drags on this timeline and we are applying an interpolated seek on the clock, until interrupted by 2 or 3.
                // 2) The user changes the track time through some other means (scrolling in the editor or overview timeline; clicking a hitobject etc.). We want the timeline to track the clock's time.
                // 3) An ongoing seek transform is running from an external seek. We want the timeline to track the clock's time.

                // The simplest way to cover the first two cases is by checking whether the scroll position has changed and the audio hasn't been changed externally
                // Checking IsSeeking covers the third case, where the transform may not have been applied yet.
                if (Current != lastScrollPosition && editorClock.CurrentTime == lastTrackTime && !editorClock.IsSeeking)
                    seekTrackToCurrent();
                else
                    scrollToTrackTime();
            }

            lastScrollPosition = Current;
            lastTrackTime = editorClock.CurrentTime;
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (!base.OnMouseDown(e))
                return false;

            beginUserDrag();
            return true;
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            endUserDrag();
            base.OnMouseUp(e);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, ITimeTagModeState timeTagModeState, KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
        {
            BindableZoom.BindTo(timeTagModeState.BindableRecordZoom);

            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.RecordingTimeTagShowWaveform, ShowWaveformGraph);
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.RecordingTimeTagWaveformOpacity, WaveformOpacity);
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.RecordingTimeTagShowTick, ShowTick);
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.RecordingTimeTagTickOpacity, TickOpacity);

            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    Name = "Background",
                    Depth = 1,
                    Y = 10,
                    RelativeSizeAxes = Axes.X,
                    Height = TIMELINE_HEIGHT,
                    Colour = colours.Gray3
                },
                trackTimer = new OsuSpriteText
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomLeft,
                    Colour = colours.Red,
                    X = 5,
                    Font = OsuFont.GetFont(size: 16, fixedWidth: true)
                }
            });
        }

        private void seekTrackToCurrent()
        {
            if (!Track.IsLoaded)
                return;

            double target = Current / Content.DrawWidth * Track.Length;
            editorClock.Seek(Math.Min(Track.Length, target));
        }

        private void scrollToTrackTime()
        {
            if (!Track.IsLoaded || Track.Length == 0)
                return;

            // covers the case where the user starts playback after a drag is in progress.
            // we want to ensure the clock is always stopped during drags to avoid weird audio playback.
            if (handlingDragInput)
                editorClock.Stop();

            ScrollTo((float)(editorClock.CurrentTime / editorClock.TrackLength) * Content.DrawWidth, false);
        }

        private void beginUserDrag()
        {
            handlingDragInput = true;
            trackWasPlaying = editorClock.IsRunning;
            editorClock.Stop();
        }

        private void endUserDrag()
        {
            handlingDragInput = false;
            if (trackWasPlaying)
                editorClock.Start();
        }
    }
}
