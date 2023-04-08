// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Preview;

public class PreviewStageInfo : StageInfo
{
    #region Category

    /// <summary>
    /// Category to save the <see cref="Lyric"/>'s and <see cref="Note"/>'s style.
    /// </summary>
    [JsonIgnore]
    private PreviewStyleCategory styleCategory { get; set; } = new();

    /// <summary>
    /// The definition for the <see cref="Lyric"/>.
    /// Like how many lyrics can in the playfield at the same time.
    /// </summary>
    public PreviewLyricLayoutDefinition LyricLayoutDefinition { get; set; } = new();

    /// <summary>
    /// Category to save the <see cref="Lyric"/>'s layout.
    /// </summary>
    [JsonIgnore]
    private PreviewLyricLayoutCategory layoutCategory { get; set; } = new();

    #endregion

    #region Init

    // todo: make the method more generic for those stages that need the beatmap.
    public override void ReloadBeatmap(IBeatmap beatmap)
    {
        var calculator = new PreviewStageTimingCalculator(beatmap);

        // also, clear all mapping in the layout and re-create one.
        layoutCategory.ClearElements();

        foreach (var lyric in beatmap.HitObjects.OfType<Lyric>())
        {
            var element = layoutCategory.AddElement(x =>
            {
                x.Name = $"Auto-generated layout with lyric {lyric.ID}";
                x.StartTime = calculator.CalculateStartTime(lyric);
                x.EndTime = calculator.CalculateEndTime(lyric);
                x.Timings = calculator.CalculateTimings(lyric);
            });
            layoutCategory.AddToMapping(element, lyric);
        }
    }

    #endregion

    #region Stage element

    protected override IEnumerable<StageElement> GetLyricStageElements(Lyric lyric)
    {
        yield return styleCategory.GetElementByItem(lyric);
        yield return layoutCategory.GetElementByItem(lyric);
    }

    protected override IEnumerable<StageElement> GetNoteStageElements(Note note)
    {
        // todo: should check the real-time mapping result.
        yield return styleCategory.GetElementByItem(note.ReferenceLyric!);
    }

    protected override IEnumerable<object> ConvertToLyricStageAppliers(IEnumerable<StageElement> elements)
    {
        throw new NotImplementedException();
    }

    protected override IEnumerable<object> ConvertToNoteStageAppliers(IEnumerable<StageElement> elements)
    {
        throw new NotImplementedException();
    }

    protected override Tuple<double?, double?> GetStartAndEndTime(Lyric lyric)
    {
        var element = layoutCategory.GetElementByItem(lyric);
        return new Tuple<double?, double?>(element.StartTime, element.EndTime);
    }

    #endregion
}