// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Zh
{
    public class ZhTimeTagGenerator : TimeTagGenerator<ZhTimeTagGeneratorConfig>
    {
        public ZhTimeTagGenerator(ZhTimeTagGeneratorConfig config)
            : base(config)
        {
        }

        protected override void TimeTagLogic(Lyric lyric, List<TimeTag> timeTags)
        {
            string text = lyric.Text;

            for (int i = 1; i < text.Length; i++)
            {
                char c = text[i];

                if (CharUtils.IsChinese(c))
                {
                    timeTags.Add(new TimeTag(new TextIndex(i)));
                }
                else if (CharUtils.IsSpacing(c) && !Config.CheckWhiteSpace)
                {
                    timeTags.Add(new TimeTag(new TextIndex(i)));
                }
            }
        }
    }
}
