﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Shaders;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;

/// <summary>
/// This karaoke skin is using in lyric editor only.
/// </summary>
public class LyricEditorSkin : KaraokeSkin
{
    public const int MIN_FONT_SIZE = 10;
    public const int MAX_FONT_SIZE = 45;

    internal static readonly Guid DEFAULT_SKIN = new("FEC5A290-5709-11EC-9F10-0800200C9A66");

    public static SkinInfo CreateInfo() => new()
    {
        ID = DEFAULT_SKIN,
        Name = "karaoke! (default editor skin)",
        Creator = "team karaoke!",
        Protected = true,
        InstantiationInfo = typeof(LyricEditorSkin).GetInvariantInstantiationInfo(),
    };

    public LyricEditorSkin(IStorageResourceProvider? resources)
        : this(CreateInfo(), resources)
    {
    }

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
    public LyricEditorSkin(SkinInfo skin, IStorageResourceProvider? resources)
        : base(skin, resources)
    {
        DefaultElement[ElementType.LyricFontInfo] = LyricFontInfo.CreateDefault();
        DefaultElement[ElementType.NoteStyle] = NoteStyle.CreateDefault();

        // todo: should use better way to handle overall size.
        FontSize = 26;
    }

    public float FontSize
    {
        get;
        set;
    }
}
