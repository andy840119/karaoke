﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics.Carets;

public abstract partial class DrawableCaret<TCaretPosition> : DrawableCaret
    where TCaretPosition : struct, ICaretPosition
{
    protected DrawableCaret(DrawableCaretState state)
        : base(state)
    {
    }

    public sealed override void ApplyCaretPosition(ICaretPosition caret)
    {
        if (caret is not TCaretPosition tCaret)
            throw new InvalidCastException();

        ApplyCaretPosition(tCaret);
    }

    protected abstract void ApplyCaretPosition(TCaretPosition caret);
}

public abstract partial class DrawableCaret : CompositeDrawable
{
    [Resolved]
    protected OsuColour Colours { get; private set; } = null!;

    [Resolved]
    protected IPreviewLyricPositionProvider LyricPositionProvider { get; private set; } = null!;

    public readonly DrawableCaretState State;

    protected DrawableCaret(DrawableCaretState state)
    {
        State = state;
    }

    protected static float GetAlpha(DrawableCaretState state) =>
        state switch
        {
            DrawableCaretState.Idle => 1,
            DrawableCaretState.Hover => 0.5f,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null),
        };

    public abstract void ApplyCaretPosition(ICaretPosition caret);

    public void TriggerDisallowEditEffect()
    {
        TriggerDisallowEditEffect(Colours);
    }

    protected abstract void TriggerDisallowEditEffect(OsuColour colour);
}
