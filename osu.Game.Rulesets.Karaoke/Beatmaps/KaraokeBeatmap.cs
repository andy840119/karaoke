﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps
{
    public class KaraokeBeatmap : Beatmap<KaraokeHitObject>
    {
        public List<CultureInfo> AvailableTranslates { get; set; } = new();

        public List<Singer> Singers { get; set; } = new();

        public int TotalColumns { get; set; } = 9;

        public override IEnumerable<BeatmapStatistic> GetStatistics()
        {
            int singers = Singers.Count;
            int lyrics = HitObjects.Count(s => s is Lyric);

            var defaultStatistic = new List<BeatmapStatistic>
            {
                new()
                {
                    Name = @"Singer",
                    Content = singers.ToString(),
                    CreateIcon = () => new SpriteIcon { Icon = FontAwesome.Solid.User }
                },
                new()
                {
                    Name = @"Lyric",
                    Content = lyrics.ToString(),
                    CreateIcon = () => new SpriteIcon { Icon = FontAwesome.Solid.AlignLeft }
                }
            };

            bool scorable = this.IsScorable();

            if (scorable)
            {
                int notes = HitObjects.Count(s => s is Note { Display: true });
                defaultStatistic.Add(new BeatmapStatistic
                {
                    Name = @"Note",
                    Content = notes.ToString(),
                    CreateIcon = () => new SpriteIcon { Icon = FontAwesome.Solid.Music }
                });
            }
            else
            {
                defaultStatistic.Add(new BeatmapStatistic
                {
                    Name = @"This beatmap is not scorable.",
                    Content = @"This beatmap is not scorable.",
                    CreateIcon = () => new SpriteIcon { Icon = FontAwesome.Solid.Times }
                });
            }

            return defaultStatistic.ToArray();
        }
    }
}
