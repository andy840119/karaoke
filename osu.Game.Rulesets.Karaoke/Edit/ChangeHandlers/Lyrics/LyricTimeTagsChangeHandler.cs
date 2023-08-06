// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romajies;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.TimeTags;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;

public partial class LyricTimeTagsChangeHandler : LyricPropertyChangeHandler, ILyricTimeTagsChangeHandler
{
    #region Auto-Generate

    public bool CanGenerate(TimeTagGeneratorType type)
    {
        switch (type)
        {
            case TimeTagGeneratorType.TimeTag:
            {
                var generator = GetSelector<TimeTag[], TimeTagGeneratorConfig>();
                return CanGenerate(generator);
            }

            case TimeTagGeneratorType.Romaji:
            {
                var generator = GetSelector<IReadOnlyDictionary<TimeTag, RomajiGenerateResult>, RomajiGeneratorConfig>();
                return CanGenerate(generator);
            }

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public IDictionary<Lyric, LocalisableString> GetGeneratorNotSupportedLyrics(TimeTagGeneratorType type)
    {
        switch (type)
        {
            case TimeTagGeneratorType.TimeTag:
            {
                var generator = GetSelector<TimeTag[], TimeTagGeneratorConfig>();
                return GetInvalidMessageFromGenerator(generator);
            }

            case TimeTagGeneratorType.Romaji:
            {
                var generator = GetSelector<IReadOnlyDictionary<TimeTag, RomajiGenerateResult>, RomajiGeneratorConfig>();
                return GetInvalidMessageFromGenerator(generator);
            }

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public void AutoGenerate(TimeTagGeneratorType type)
    {
        switch (type)
        {
            case TimeTagGeneratorType.TimeTag:
            {
                var generator = GetSelector<TimeTag[], TimeTagGeneratorConfig>();

                PerformOnSelection(lyric =>
                {
                    lyric.TimeTags = generator.Generate(lyric);
                });

                break;
            }

            case TimeTagGeneratorType.Romaji:
            {
                var generator = GetSelector<IReadOnlyDictionary<TimeTag, RomajiGenerateResult>, RomajiGeneratorConfig>();

                PerformOnSelection(lyric =>
                {
                    var result = generator.Generate(lyric);

                    foreach (var timeTag in lyric.TimeTags)
                    {
                        timeTag.InitialRomaji = result[timeTag].InitialRomaji;
                        timeTag.RomajiText = result[timeTag].RomajiText;
                    }
                });

                break;
            }

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    #endregion

    public void SetTimeTagTime(TimeTag timeTag, double time)
    {
        CheckExactlySelectedOneHitObject();

        PerformOnSelection(lyric =>
        {
            bool containsInLyric = lyric.TimeTags.Contains(timeTag);
            if (!containsInLyric)
                throw new InvalidOperationException($"{nameof(timeTag)} is not in the lyric");

            timeTag.Time = time;
        });
    }

    public void SetTimeTagInitialRomaji(TimeTag timeTag, bool initialRomaji)
    {
        CheckExactlySelectedOneHitObject();

        PerformOnSelection(lyric =>
        {
            bool containsInLyric = lyric.TimeTags.Contains(timeTag);
            if (!containsInLyric)
                throw new InvalidOperationException($"{nameof(timeTag)} is not in the lyric");

            timeTag.InitialRomaji = initialRomaji;
        });
    }

    public void SetTimeTagRomajiText(TimeTag timeTag, string romaji)
    {
        CheckExactlySelectedOneHitObject();

        PerformOnSelection(lyric =>
        {
            bool containsInLyric = lyric.TimeTags.Contains(timeTag);
            if (!containsInLyric)
                throw new InvalidOperationException($"{nameof(timeTag)} is not in the lyric");

            timeTag.RomajiText = romaji;

            if (!string.IsNullOrWhiteSpace(romaji))
                return;

            timeTag.RomajiText = string.Empty;
            timeTag.InitialRomaji = false;
        });
    }

    public void ShiftingTimeTagTime(IEnumerable<TimeTag> timeTags, double offset)
    {
        CheckExactlySelectedOneHitObject();
        NotTriggerSaveStateOnThisChange();

        PerformOnSelection(lyric =>
        {
            foreach (var timeTag in timeTags)
            {
                bool containsInLyric = lyric.TimeTags.Contains(timeTag);
                if (!containsInLyric)
                    throw new InvalidOperationException($"{nameof(timeTag)} is not in the lyric");

                timeTag.Time += offset;
            }
        });
    }

    public void ClearTimeTagTime(TimeTag timeTag)
    {
        CheckExactlySelectedOneHitObject();

        PerformOnSelection(lyric =>
        {
            bool containsInLyric = lyric.TimeTags.Contains(timeTag);
            if (!containsInLyric)
                throw new InvalidOperationException($"{nameof(timeTag)} is not in the lyric");

            timeTag.Time = null;
        });
    }

    public void ClearAllTimeTagTime()
    {
        PerformOnSelection(lyric =>
        {
            foreach (var timeTag in lyric.TimeTags)
            {
                timeTag.Time = null;
            }
        });
    }

    public void Add(TimeTag timeTag)
    {
        CheckExactlySelectedOneHitObject();

        PerformOnSelection(lyric =>
        {
            bool containsInLyric = lyric.TimeTags.Contains(timeTag);
            if (containsInLyric)
                throw new InvalidOperationException($"{nameof(timeTag)} already in the lyric");

            insertTimeTag(lyric, timeTag, InsertDirection.End);
        });
    }

    public void AddRange(IEnumerable<TimeTag> timeTags)
    {
        CheckExactlySelectedOneHitObject();

        PerformOnSelection(lyric =>
        {
            // should convert to array because enumerable might change while deleting.
            foreach (var timeTag in timeTags.ToArray())
            {
                bool containsInLyric = lyric.TimeTags.Contains(timeTag);
                if (containsInLyric)
                    throw new InvalidOperationException($"{nameof(timeTag)} already in the lyric");

                insertTimeTag(lyric, timeTag, InsertDirection.End);
            }
        });
    }

    public void Remove(TimeTag timeTag)
    {
        CheckExactlySelectedOneHitObject();

        PerformOnSelection(lyric =>
        {
            // delete time tag from list
            lyric.TimeTags.Remove(timeTag);
        });
    }

    public void RemoveRange(IEnumerable<TimeTag> timeTags)
    {
        CheckExactlySelectedOneHitObject();

        PerformOnSelection(lyric =>
        {
            // should convert to array because enumerable might change while deleting.
            foreach (var timeTag in timeTags.ToArray())
            {
                bool containsInLyric = lyric.TimeTags.Remove(timeTag);
                if (!containsInLyric)
                    throw new InvalidOperationException($"{nameof(timeTag)} is not in the lyric");
            }
        });
    }

    public void AddByPosition(TextIndex index)
    {
        CheckExactlySelectedOneHitObject();

        PerformOnSelection(lyric =>
        {
            insertTimeTag(lyric, new TimeTag(index), InsertDirection.End);
        });
    }

    public void RemoveByPosition(TextIndex index)
    {
        CheckExactlySelectedOneHitObject();

        PerformOnSelection(lyric =>
        {
            var matchedTimeTags = lyric.TimeTags.Where(x => x.Index == index).ToList();
            if (!matchedTimeTags.Any())
                return;

            var removedTimeTag = matchedTimeTags.MinBy(x => x.Time ?? double.MinValue);
            if (removedTimeTag != null)
                lyric.TimeTags.Remove(removedTimeTag);
        });
    }

    public TimeTag Shifting(TimeTag timeTag, ShiftingDirection direction, ShiftingType type)
    {
        CheckExactlySelectedOneHitObject();

        TimeTag newTimeTag = null!;

        PerformOnSelection(lyric =>
        {
            bool containsInLyric = lyric.TimeTags.Contains(timeTag);
            if (!containsInLyric)
                throw new InvalidOperationException($"{nameof(timeTag)} is not in the lyric");

            // remove the time-tag first.
            lyric.TimeTags.Remove(timeTag);

            // then, create a new one and insert into the list.
            var newIndex = calculateNewIndex(lyric, timeTag.Index, direction, type);
            double? newTime = timeTag.Time;
            newTimeTag = new TimeTag(newIndex, newTime);

            switch (direction)
            {
                case ShiftingDirection.Left:
                    insertTimeTag(lyric, newTimeTag, InsertDirection.End);
                    break;

                case ShiftingDirection.Right:
                    insertTimeTag(lyric, newTimeTag, InsertDirection.Start);
                    break;

                default:
                    throw new InvalidOperationException();
            }
        });

        return newTimeTag;

        static TextIndex calculateNewIndex(Lyric lyric, TextIndex originIndex, ShiftingDirection direction, ShiftingType type)
        {
            var newIndex = getNewIndex(originIndex, direction, type);
            if (TextIndexUtils.OutOfRange(newIndex, lyric.Text))
                throw new ArgumentOutOfRangeException();

            return newIndex;

            static TextIndex getNewIndex(TextIndex originIndex, ShiftingDirection direction, ShiftingType type) =>
                type switch
                {
                    ShiftingType.Index => TextIndexUtils.ShiftingIndex(originIndex, direction == ShiftingDirection.Left ? -1 : 1),
                    ShiftingType.State => direction == ShiftingDirection.Left ? TextIndexUtils.GetPreviousIndex(originIndex) : TextIndexUtils.GetNextIndex(originIndex),
                    _ => throw new InvalidOperationException(),
                };
        }
    }

    private void insertTimeTag(Lyric lyric, TimeTag timeTag, InsertDirection direction)
    {
        var timeTags = lyric.TimeTags;

        // just add if there's no time-tag
        if (lyric.TimeTags.Count == 0)
        {
            timeTags.Add(timeTag);
            return;
        }

        if (timeTags.All(x => x.Index < timeTag.Index))
        {
            timeTags.Add(timeTag);
        }
        else if (timeTags.All(x => x.Index > timeTag.Index))
        {
            timeTags.Insert(0, timeTag);
        }
        else
        {
            switch (direction)
            {
                case InsertDirection.Start:
                {
                    var nextTimeTag = timeTags.FirstOrDefault(x => x.Index >= timeTag.Index) ?? timeTags.Last();
                    int index = timeTags.IndexOf(nextTimeTag);
                    timeTags.Insert(index, timeTag);
                    break;
                }

                case InsertDirection.End:
                {
                    var previousTextTag = timeTags.Reverse().FirstOrDefault(x => x.Index <= timeTag.Index) ?? timeTags.First();
                    int index = timeTags.IndexOf(previousTextTag) + 1;
                    timeTags.Insert(index, timeTag);
                    break;
                }

                default:
                    throw new InvalidOperationException();
            }
        }
    }

    protected override bool IsWritePropertyLocked(Lyric lyric)
        => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.TimeTags));

    /// <summary>
    /// Insert direction if contains the time-tag with the same index.
    /// </summary>
    private enum InsertDirection
    {
        Start,

        End,
    }
}
