﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags
{
    public abstract class RomajiTagGenerator<T> : RomajiTagGenerator where T : RomajiTagGeneratorConfig
    {
        protected T Config { get; }

        protected RomajiTagGenerator(T config)
        {
            Config = config;
        }
    }

    public abstract class RomajiTagGenerator : ILyricPropertyGenerator<RomajiTag[]>
    {
        public bool CanGenerate(Lyric lyric)
            => !string.IsNullOrWhiteSpace(lyric.Text);

        public abstract RomajiTag[] Generate(Lyric lyric);
    }
}
