﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;
using System;

namespace osu.Game.Rulesets.Karaoke.Overlays.Changelog
{
    public class ChangelogContent : FillFlowContainer
    {
        public Action<KaraokeChangelogBuild> BuildSelected;
    }
}
