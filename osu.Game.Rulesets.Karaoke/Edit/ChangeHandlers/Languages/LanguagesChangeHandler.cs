// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Languages
{
    public class LanguagesChangeHandler : BeatmapPropertyChangeHandler<CultureInfo>, ILanguagesChangeHandler
    {
        public IBindableList<CultureInfo> Languages => Items;

        public bool IsLanguageContainsTranslate(CultureInfo cultureInfo)
        {
            return Lyrics.Any(x => x.Translates.ContainsKey(cultureInfo));
        }

        protected override List<CultureInfo> GetItemsFromBeatmap(KaraokeBeatmap beatmap)
        {
            return beatmap.AvailableTranslates;
        }

        protected override void OnItemAdded(CultureInfo item)
        {
            // there's no need to do anything.
        }

        protected override void OnItemRemoved(CultureInfo item)
        {
            // Delete from lyric also.
            foreach (var lyric in Lyrics.Where(lyric => lyric.Translates.ContainsKey(item))) lyric.Translates.Remove(item);
        }
    }
}
