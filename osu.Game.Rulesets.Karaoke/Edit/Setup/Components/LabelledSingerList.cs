﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Edit.Setup.Components;

public partial class LabelledSingerList : LabelledDrawable<SingerList>
{
    public LabelledSingerList()
        : base(true)
    {
    }

    public BindableList<Singer> Singers => Component.Singers;

    protected override SingerList CreateComponent() => new();
}
