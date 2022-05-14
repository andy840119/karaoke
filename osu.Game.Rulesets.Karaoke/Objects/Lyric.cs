﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Extensions;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Judgements;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class Lyric : KaraokeHitObject, IHasDuration, IHasSingers, IHasOrder, IHasLock, IHasPrimaryKey
    {
        [JsonIgnore]
        public readonly Bindable<string> TextBindable = new();

        [JsonIgnore]
        public readonly Bindable<int> TimeTagsVersion = new();

        [JsonIgnore]
        public readonly BindableList<TimeTag> TimeTagsBindable = new();

        [JsonIgnore]
        public double LyricDuration => LyricEndTime - LyricStartTime;

        [JsonIgnore]
        public readonly Bindable<int> RubyTagsVersion = new();

        [JsonIgnore]
        public readonly BindableList<RubyTag> RubyTagsBindable = new();

        [JsonIgnore]
        public readonly Bindable<int> RomajiTagsVersion = new();

        [JsonIgnore]
        public readonly BindableList<RomajiTag> RomajiTagsBindable = new();

        /// <summary>
        ///     The time at which the HitObject end.
        /// </summary>
        public double EndTime => StartTime + Duration;

        [JsonIgnore]
        public readonly BindableList<int> SingersBindable = new();

        [JsonIgnore]
        public readonly BindableDictionary<CultureInfo, string> TranslateTextBindable = new();

        [JsonIgnore]
        public readonly Bindable<CultureInfo> LanguageBindable = new();

        [JsonIgnore]
        public readonly Bindable<int> OrderBindable = new();

        [JsonIgnore]
        public readonly Bindable<LockState> LockBindable = new();

        /// <summary>
        ///     Primary key.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///     Text of the lyric
        /// </summary>
        public string Text
        {
            get => TextBindable.Value;
            set => TextBindable.Value = value;
        }

        /// <summary>
        ///     Time tags
        /// </summary>
        public IList<TimeTag> TimeTags
        {
            get => TimeTagsBindable;
            set
            {
                TimeTagsBindable.Clear();
                TimeTagsBindable.AddRange(value);

                // todo: it might not a good idea to set the time in here.
                LyricStartTime = TimeTagsUtils.GetStartTime(value) ?? StartTime;
                LyricEndTime = TimeTagsUtils.GetEndTime(value) ?? EndTime;
            }
        }

        [JsonIgnore]
        public double LyricStartTime { get; private set; }

        [JsonIgnore]
        public double LyricEndTime { get; private set; }

        /// <summary>
        ///     List of ruby tags
        /// </summary>
        public IList<RubyTag> RubyTags
        {
            get => RubyTagsBindable;
            set
            {
                RubyTagsBindable.Clear();
                RubyTagsBindable.AddRange(value);
            }
        }

        /// <summary>
        ///     List of ruby tags
        /// </summary>
        public IList<RomajiTag> RomajiTags
        {
            get => RomajiTagsBindable;
            set
            {
                RomajiTagsBindable.Clear();
                RomajiTagsBindable.AddRange(value);
            }
        }

        /// <summary>
        ///     Lyric's start time is created from <see cref="KaraokeBeatmapProcessor" /> and should not be saved.
        /// </summary>
        public override double StartTime
        {
            get => base.StartTime;
            set => base.StartTime = value;
        }

        /// <summary>
        ///     Lyric's duration is created from <see cref="KaraokeBeatmapProcessor" /> and should not be saved.
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        ///     Singers
        /// </summary>
        public IList<int> Singers
        {
            get => SingersBindable;
            set
            {
                SingersBindable.Clear();
                SingersBindable.AddRange(value);
            }
        }

        /// <summary>
        ///     Translates
        /// </summary>
        public IDictionary<CultureInfo, string> Translates
        {
            get => TranslateTextBindable;
            set
            {
                TranslateTextBindable.Clear();
                TranslateTextBindable.AddRange(value);
            }
        }

        /// <summary>
        ///     Language
        /// </summary>
        public CultureInfo Language
        {
            get => LanguageBindable.Value;
            set => LanguageBindable.Value = value;
        }

        /// <summary>
        ///     Order
        /// </summary>
        public int Order
        {
            get => OrderBindable.Value;
            set => OrderBindable.Value = value;
        }

        /// <summary>
        ///     Lock
        /// </summary>
        public LockState Lock
        {
            get => LockBindable.Value;
            set => LockBindable.Value = value;
        }

        public Lyric()
        {
            TimeTagsBindable.CollectionChanged += (_, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var c in args.NewItems.Cast<TimeTag>())
                            c.Changed += invalidate;
                        break;

                    case NotifyCollectionChangedAction.Reset:
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var c in args.OldItems.Cast<TimeTag>())
                            c.Changed -= invalidate;
                        break;
                }

                void invalidate() => TimeTagsVersion.Value++;
            };

            RubyTagsBindable.CollectionChanged += (_, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var c in args.NewItems.Cast<RubyTag>())
                            c.Changed += invalidate;
                        break;

                    case NotifyCollectionChangedAction.Reset:
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var c in args.OldItems.Cast<RubyTag>())
                            c.Changed -= invalidate;
                        break;
                }

                void invalidate() => RubyTagsVersion.Value++;
            };

            RomajiTagsBindable.CollectionChanged += (_, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var c in args.NewItems.Cast<RomajiTag>())
                            c.Changed += invalidate;
                        break;

                    case NotifyCollectionChangedAction.Reset:
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var c in args.OldItems.Cast<RomajiTag>())
                            c.Changed -= invalidate;
                        break;
                }

                void invalidate() => RomajiTagsVersion.Value++;
            };
        }

        public override Judgement CreateJudgement()
        {
            return new KaraokeLyricJudgement();
        }

        public void InitialWorkingTime()
        {
            StartTime = LyricStartTime;
            Duration = LyricDuration;
        }

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, IBeatmapDifficultyInfo difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

            // Add because it will cause error on exit then enter gameplay.
            StartTimeBindable.UnbindAll();

            // Initial working start and end time.
            InitialWorkingTime();
        }

        protected override HitWindows CreateHitWindows()
        {
            return new KaraokeLyricHitWindows();
        }
    }
}
