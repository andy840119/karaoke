﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Edit.Checker.Types
{
    public interface IHasConfig<out T> where T : new()
    {
        public T CreateDefaultConfig();
    }
}