// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Stages.Infos.Preview;

public class PreviewStyleCategory : StageElementCategory<PreviewStyle, Lyric>
{
    protected override PreviewStyle CreateDefaultElement()
        => new()
        {
            LyricStyle = LyricStyle.CreateDefault(),
            LyricFontInfo = LyricFontInfo.CreateDefault(),
        };
}
