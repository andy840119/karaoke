// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Preview;

public class PreviewStyleCategory : StageElementCategory<PreviewStyle, Lyric>
{
    protected override PreviewStyle CreateElement(int id) => new(id);
}