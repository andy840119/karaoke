﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Judgements;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class Note : KaraokeHitObject, IHasDuration, IHasText
    {
        [JsonIgnore]
        public readonly Bindable<string> TextBindable = new();

        /// <summary>
        /// Text display on the note.
        /// </summary>
        /// <example>
        /// 花
        /// </example>
        public string Text
        {
            get => TextBindable.Value;
            set => TextBindable.Value = value;
        }

        [JsonIgnore]
        public readonly Bindable<string> AlternativeTextBindable = new();

        /// <summary>
        /// Alternative text.
        /// Should placing something like ruby, 拼音 or ふりがな.
        /// Will be display only if <see cref="KaraokeRulesetSetting.DisplayAlternativeText"/> is true.
        /// </summary>
        /// <example>
        /// はな
        /// </example>
        public string AlternativeText
        {
            get => AlternativeTextBindable.Value;
            set => AlternativeTextBindable.Value = value;
        }

        [JsonIgnore]
        public readonly Bindable<bool> DisplayBindable = new();

        /// <summary>
        /// Display this note
        /// </summary>
        public bool Display
        {
            get => DisplayBindable.Value;
            set => DisplayBindable.Value = value;
        }

        [JsonIgnore]
        public readonly Bindable<Tone> ToneBindable = new();

        /// <summary>
        /// Tone of this note
        /// </summary>
        public virtual Tone Tone
        {
            get => ToneBindable.Value;
            set => ToneBindable.Value = value;
        }

        /// <summary>
        /// Duration
        /// </summary>
        [JsonIgnore]
        public double Duration { get; set; }

        /// <summary>
        /// The time at which the HitObject end.
        /// </summary>
        [JsonIgnore]
        public double EndTime => StartTime + Duration;

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        [JsonIgnore]
        public readonly Bindable<Lyric> ParentLyricBindable = new();

        /// <summary>
        /// Relative lyric.
        /// Technically parent lyric will not change after assign, but should not restrict in model layer.
        /// </summary>
        [JsonProperty(IsReference = true)]
        public Lyric ParentLyric
        {
            get => ParentLyricBindable.Value;
            set => ParentLyricBindable.Value = value;
        }

        public override Judgement CreateJudgement() => new KaraokeNoteJudgement();
    }
}
