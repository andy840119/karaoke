﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Judgements;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class Note : KaraokeHitObject, IHasDuration, IHasText
    {
        [JsonIgnore]
        public readonly Bindable<string> TextBindable = new();

        [JsonIgnore]
        public readonly Bindable<string> RubyTextBindable = new();

        [JsonIgnore]
        public readonly Bindable<bool> DisplayBindable = new();

        [JsonIgnore]
        public readonly Bindable<Tone> ToneBindable = new();

        /// <summary>
        ///     The time at which the HitObject end.
        /// </summary>
        [JsonIgnore]
        public double EndTime => StartTime + Duration;

        /// <summary>
        ///     Text display on the note.
        /// </summary>
        /// <example>
        ///     花
        /// </example>
        public string Text
        {
            get => TextBindable.Value;
            set => TextBindable.Value = value;
        }

        /// <summary>
        ///     Ruby text.
        ///     Should placing something like ruby, 拼音 or ふりがな.
        ///     Will be display only if <see cref="KaraokeRulesetSetting.DisplayNoteRubyText" /> is true.
        /// </summary>
        /// <example>
        ///     はな
        /// </example>
        public string RubyText
        {
            get => RubyTextBindable.Value;
            set => RubyTextBindable.Value = value;
        }

        /// <summary>
        ///     Display this note
        /// </summary>
        public bool Display
        {
            get => DisplayBindable.Value;
            set => DisplayBindable.Value = value;
        }

        /// <summary>
        ///     Tone of this note
        /// </summary>
        public virtual Tone Tone
        {
            get => ToneBindable.Value;
            set => ToneBindable.Value = value;
        }

        /// <summary>
        ///     Duration
        /// </summary>
        [JsonIgnore]
        public double Duration { get; set; }

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        /// <summary>
        ///     Relative lyric.
        ///     Technically parent lyric will not change after assign, but should not restrict in model layer.
        /// </summary>
        [JsonProperty(IsReference = true)]
        public Lyric ParentLyric { get; set; }

        public override Judgement CreateJudgement()
        {
            return new KaraokeNoteJudgement();
        }

        protected override HitWindows CreateHitWindows()
        {
            return new KaraokeNoteHitWindows();
        }
    }
}
