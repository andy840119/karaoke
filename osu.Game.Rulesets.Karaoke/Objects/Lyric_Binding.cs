﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    /// <summary>
    /// Placing the binding-related logic.
    /// </summary>
    public partial class Lyric
    {
        private void initInternalBindingEvent()
        {
            TimeTagsBindable.CollectionChanged += (_, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        Debug.Assert(args.NewItems != null);

                        foreach (var c in args.NewItems.Cast<TimeTag>())
                            c.Changed += invalidate;
                        break;

                    case NotifyCollectionChangedAction.Reset:
                    case NotifyCollectionChangedAction.Remove:
                        Debug.Assert(args.OldItems != null);

                        foreach (var c in args.OldItems.Cast<TimeTag>())
                            c.Changed -= invalidate;
                        break;
                }

                updateLyricTime();

                void invalidate() => timeTagsVersion.Value++;
            };

            TimeTagsVersion.ValueChanged += _ =>
            {
                updateLyricTime();
            };

            RubyTagsBindable.CollectionChanged += (_, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        Debug.Assert(args.NewItems != null);

                        foreach (var c in args.NewItems.Cast<RubyTag>())
                            c.Changed += invalidate;
                        break;

                    case NotifyCollectionChangedAction.Reset:
                    case NotifyCollectionChangedAction.Remove:
                        Debug.Assert(args.OldItems != null);

                        foreach (var c in args.OldItems.Cast<RubyTag>())
                            c.Changed -= invalidate;
                        break;
                }

                void invalidate() => rubyTagsVersion.Value++;
            };

            RomajiTagsBindable.CollectionChanged += (_, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        Debug.Assert(args.NewItems != null);

                        foreach (var c in args.NewItems.Cast<RomajiTag>())
                            c.Changed += invalidate;
                        break;

                    case NotifyCollectionChangedAction.Reset:
                    case NotifyCollectionChangedAction.Remove:
                        Debug.Assert(args.OldItems != null);

                        foreach (var c in args.OldItems.Cast<RomajiTag>())
                            c.Changed -= invalidate;
                        break;
                }

                void invalidate() => romajiTagsVersion.Value++;
            };

            LockBindable.ValueChanged += e =>
            {
                lyricPropertyWritableVersion.Value++;
            };

            ReferenceLyricConfigBindable.ValueChanged += e =>
            {
                if (e.OldValue != null)
                {
                    e.OldValue.Changed -= invalidate;
                }

                if (e.NewValue != null)
                {
                    e.NewValue.Changed += invalidate;
                }

                void invalidate() => referenceLyricConfigVersion.Value++;
            };

            void updateLyricTime()
            {
                // todo: should think about lyric time should be nullable?
                LyricStartTime = TimeTagsUtils.GetStartTime(TimeTags) ?? StartTime;
                LyricDuration = TimeTagsUtils.GetDuration(TimeTags) ?? Duration;
            }
        }

        private void initReferenceLyricEvent()
        {
            ReferenceLyricConfigVersion.ValueChanged += e =>
            {
                ReferenceLyricBindable.TriggerChange();
            };

            ReferenceLyricConfigBindable.ValueChanged += e =>
            {
                ReferenceLyricBindable.TriggerChange();
            };

            ReferenceLyricBindable.ValueChanged += e =>
            {
                lyricPropertyWritableVersion.Value++;

                // text.
                bindValueChange(e, l => l.TextBindable, (lyric, config) =>
                {
                    if (config is not SyncLyricConfig)
                        return;

                    Text = lyric.Text;
                });

                // time-tags.
                bindListValueChange(e, l => l.TimeTagsBindable, (lyric, config) =>
                {
                    if (config is not SyncLyricConfig syncLyricConfig || !syncLyricConfig.SyncTimeTagProperty)
                        return;

                    TimeTags = lyric.TimeTags.Select(x =>
                    {
                        var newTimeTag = x.DeepClone();
                        newTimeTag.Time += config.OffsetTime;
                        return newTimeTag;
                    }).ToArray();
                });

                bindValueChange(e, l => l.TimeTagsVersion, (_, config) =>
                {
                    if (config is not SyncLyricConfig syncLyricConfig || !syncLyricConfig.SyncTimeTagProperty)
                        return;

                    syncProperty(x => x.TimeTags, (from, to) =>
                    {
                        to.Time = from.Time;
                    });
                }, false);

                // todo: start time and end time?

                // ruby-tags.
                bindListValueChange(e, l => l.RubyTagsBindable, (lyric, config) =>
                {
                    if (config is not SyncLyricConfig)
                        return;

                    RubyTags = lyric.RubyTags.Select(x => x.DeepClone()).ToArray();
                });

                bindValueChange(e, l => l.RubyTagsVersion, (_, config) =>
                {
                    if (config is not SyncLyricConfig)
                        return;

                    syncProperty(x => x.RubyTags, (from, to) =>
                    {
                        to.StartIndex = from.StartIndex;
                        to.EndIndex = from.EndIndex;
                        to.Text = from.Text;
                    });
                }, false);

                // romaji-tags.
                bindListValueChange(e, l => l.RomajiTagsBindable, (lyric, config) =>
                {
                    if (config is not SyncLyricConfig)
                        return;

                    RomajiTags = lyric.RomajiTags.Select(x => x.DeepClone()).ToArray();
                });

                bindValueChange(e, l => l.RomajiTagsVersion, (_, config) =>
                {
                    if (config is not SyncLyricConfig)
                        return;

                    syncProperty(x => x.RomajiTags, (from, to) =>
                    {
                        to.StartIndex = from.StartIndex;
                        to.EndIndex = from.EndIndex;
                        to.Text = from.Text;
                    });
                }, false);

                // todo: start-time, end-time and offset.

                // singers.
                bindListValueChange(e, l => l.SingersBindable, (lyric, config) =>
                {
                    if (config is not SyncLyricConfig syncLyricConfig || !syncLyricConfig.SyncSingerProperty)
                        return;

                    Singers = lyric.Singers;
                });

                // translates.
                bindDictionaryValueChange(e, l => l.TranslateTextBindable, (lyric, config) =>
                {
                    if (config is not SyncLyricConfig)
                        return;

                    Translates = lyric.Translates;
                });

                // language.
                bindValueChange(e, l => l.LanguageBindable, (lyric, config) =>
                {
                    if (config is not SyncLyricConfig)
                        return;

                    Language = lyric.Language;
                });
            };
        }

        private void bindValueChange<T>(ValueChangedEvent<Lyric?> e, Func<Lyric, IBindable<T>> getProperty, Action<Lyric, IReferenceLyricPropertyConfig> syncAction, bool triggerChangeOnBind = true)
        {
            if (e.OldValue != null)
            {
                getProperty(e.OldValue).ValueChanged -= propertyChanged;
            }

            if (e.NewValue != null)
            {
                getProperty(e.NewValue).ValueChanged += propertyChanged;

                if (triggerChangeOnBind)
                    triggerPropertyChanged();
            }

            void propertyChanged(ValueChangedEvent<T> _) => triggerPropertyChanged();

            void triggerPropertyChanged()
            {
                if (ReferenceLyricConfig == null || ReferenceLyric == null)
                    return;

                // trigger change
                syncAction(ReferenceLyric, ReferenceLyricConfig);
            }
        }

        private void bindListValueChange<T>(ValueChangedEvent<Lyric?> e, Func<Lyric, IBindableList<T>> getProperty, Action<Lyric, IReferenceLyricPropertyConfig> syncAction,
                                            bool triggerChangeOnBind = true)
        {
            if (e.OldValue != null)
            {
                getProperty(e.OldValue).CollectionChanged -= propertyChanged;
            }

            if (e.NewValue != null)
            {
                getProperty(e.NewValue).CollectionChanged += propertyChanged;

                if (triggerChangeOnBind)
                    triggerPropertyChanged();
            }

            void propertyChanged(object? sender, NotifyCollectionChangedEventArgs _) => triggerPropertyChanged();

            void triggerPropertyChanged()
            {
                if (ReferenceLyricConfig == null || ReferenceLyric == null)
                    return;

                // trigger change
                syncAction(ReferenceLyric, ReferenceLyricConfig);
            }
        }

        private void bindDictionaryValueChange<TKey, TValue>(ValueChangedEvent<Lyric?> e, Func<Lyric, IBindableDictionary<TKey, TValue>> getProperty,
                                                             Action<Lyric, IReferenceLyricPropertyConfig> syncAction, bool triggerChangeOnBind = true)
            where TKey : notnull
        {
            if (e.OldValue != null)
            {
                getProperty(e.OldValue).CollectionChanged -= propertyChanged;
            }

            if (e.NewValue != null)
            {
                getProperty(e.NewValue).CollectionChanged += propertyChanged;

                if (triggerChangeOnBind)
                    triggerPropertyChanged();
            }

            void propertyChanged(object? sender, NotifyDictionaryChangedEventArgs<TKey, TValue> _) => triggerPropertyChanged();

            void triggerPropertyChanged()
            {
                if (ReferenceLyricConfig == null || ReferenceLyric == null)
                    return;

                // trigger change
                syncAction(ReferenceLyric, ReferenceLyricConfig);
            }
        }

        private void syncProperty<TItem>(Func<Lyric, IList<TItem>> getProperty, Action<TItem, TItem> performPaste)
        {
            Debug.Assert(ReferenceLyric != null);

            var fromList = getProperty(ReferenceLyric);
            var toList = getProperty(this);

            Debug.Assert(fromList.Count == toList.Count);

            for (int i = 0; i < fromList.Count; i++)
            {
                performPaste(fromList[i], toList[i]);
            }
        }
    }
}
