﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;

public class ClassicStyle : StageElement
{
    /// <summary>
    /// <see cref="Lyric"/>'s text style.
    /// </summary>
    public LyricStyle? LyricStyle { get; set; }

    /// <summary>
    /// <see cref="Lyric"/>'s font info.
    /// </summary>
    public LyricFontInfo? LyricFontInfo { get; set; }

    [JsonIgnore]
    public readonly Bindable<int?> NoteStyleIndexBindable = new();

    /// <summary>
    /// <see cref="Note"/>'s skin lookup index.
    /// </summary>
    public int? NoteStyleIndex
    {
        get => NoteStyleIndexBindable.Value;
        set => NoteStyleIndexBindable.Value = value;
    }
}
