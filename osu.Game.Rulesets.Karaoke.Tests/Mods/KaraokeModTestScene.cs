﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Mods
{
    public abstract class KaraokeModTestScene : ModTestScene
    {
        protected override Ruleset CreatePlayerRuleset()
        {
            return new KaraokeRuleset();
        }
    }
}
