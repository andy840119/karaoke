﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Configuration
{
    public class KaraokeRulesetLyricEditorConfigManager : InMemoryConfigManager<KaraokeRulesetLyricEditorSetting>
    {
        /// <summary>
        ///     Binds a local bindable with a configuration-backed bindable.
        /// </summary>
        public void BindWith<TValue>(KaraokeRulesetLyricEditorSetting lookup, IBindable<TValue> bindable)
        {
            bindable.BindTo(GetOriginalBindable<TValue>(lookup));
        }

        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();

            // General
            SetDefault(KaraokeRulesetLyricEditorSetting.LyricEditorFontSize, 28f);
            SetDefault(KaraokeRulesetLyricEditorSetting.AutoFocusToEditLyric, true);
            SetDefault(KaraokeRulesetLyricEditorSetting.AutoFocusToEditLyricSkipRows, 1, 0, 4);
            SetDefault(KaraokeRulesetLyricEditorSetting.ClickToLockLyricState, LockState.Partial);

            // Create time-tag.
            SetDefault(KaraokeRulesetLyricEditorSetting.CreateTimeTagEditMode, CreateTimeTagEditMode.Create);
            SetDefault(KaraokeRulesetLyricEditorSetting.CreateTimeTagMovingCaretMode, MovingTimeTagCaretMode.None);

            // Recording
            SetDefault(KaraokeRulesetLyricEditorSetting.RecordingTimeTagMovingCaretMode, MovingTimeTagCaretMode.None);
            SetDefault(KaraokeRulesetLyricEditorSetting.RecordingAutoMoveToNextTimeTag, true);
            SetDefault(KaraokeRulesetLyricEditorSetting.RecordingChangeTimeWhileMovingTheCaret, true);
            SetDefault(KaraokeRulesetLyricEditorSetting.RecordingTimeTagShowWaveform, true);
            SetDefault(KaraokeRulesetLyricEditorSetting.RecordingTimeTagWaveformOpacity, 0.5f, 0, 1, 0.01f);
            SetDefault(KaraokeRulesetLyricEditorSetting.RecordingTimeTagShowTick, true);
            SetDefault(KaraokeRulesetLyricEditorSetting.RecordingTimeTagTickOpacity, 0.5f, 0, 1, 0.01f);

            // Adjust
            SetDefault(KaraokeRulesetLyricEditorSetting.AdjustTimeTagShowWaveform, true);
            SetDefault(KaraokeRulesetLyricEditorSetting.AdjustTimeTagWaveformOpacity, 0.5f, 0, 1, 0.01f);
            SetDefault(KaraokeRulesetLyricEditorSetting.AdjustTimeTagShowTick, true);
            SetDefault(KaraokeRulesetLyricEditorSetting.AdjustTimeTagTickOpacity, 0.5f, 0, 1, 0.01f);
        }
    }

    public enum KaraokeRulesetLyricEditorSetting
    {
        // General
        LyricEditorFontSize,
        AutoFocusToEditLyric,
        AutoFocusToEditLyricSkipRows,
        ClickToLockLyricState,

        // Create time-tag.
        CreateTimeTagEditMode,
        CreateTimeTagMovingCaretMode,

        // Recording
        RecordingTimeTagMovingCaretMode,
        RecordingAutoMoveToNextTimeTag,
        RecordingChangeTimeWhileMovingTheCaret,
        RecordingTimeTagShowWaveform,
        RecordingTimeTagWaveformOpacity,
        RecordingTimeTagShowTick,
        RecordingTimeTagTickOpacity,

        // Adjust
        AdjustTimeTagShowWaveform,
        AdjustTimeTagWaveformOpacity,
        AdjustTimeTagShowTick,
        AdjustTimeTagTickOpacity
    }
}
