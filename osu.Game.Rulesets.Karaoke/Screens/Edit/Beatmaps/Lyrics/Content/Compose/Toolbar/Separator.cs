﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose.Toolbar;

public partial class Separator : CompositeDrawable
{
    private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();

    private readonly Box barLine;

    public Separator()
    {
        Size = new Vector2(3, SpecialActionToolbar.HEIGHT);
        InternalChild = barLine = new Box
        {
            RelativeSizeAxes = Axes.Both,
        };
    }

    [BackgroundDependencyLoader]
    private void load(ILyricEditorState state, LyricEditorColourProvider colourProvider)
    {
        bindableMode.BindValueChanged(x =>
        {
            barLine.Colour = colourProvider.Background1(state.Mode);
        }, true);
    }
}
