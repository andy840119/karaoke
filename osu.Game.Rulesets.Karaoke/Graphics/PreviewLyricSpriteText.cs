﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Graphics
{
    public class PreviewLyricSpriteText : LyricSpriteText
    {
        public readonly Lyric HitObject;

        public PreviewLyricSpriteText(Lyric hitObject)
        {
            HitObject = hitObject;

            hitObject.TextBindable.BindValueChanged(text => { Text = text.NewValue; }, true);
            hitObject.RubyTagsBindable.BindCollectionChanged((_, _) => { Rubies = hitObject.RubyTagsBindable.Select(x => new PositionText(x.Text, x.StartIndex, x.EndIndex)).ToArray(); }, true);
            hitObject.RomajiTagsBindable.BindCollectionChanged((_, _) => { Romajies = hitObject.RomajiTagsBindable.Select(x => new PositionText(x.Text, x.StartIndex, x.EndIndex)).ToArray(); }, true);
        }
    }
}
