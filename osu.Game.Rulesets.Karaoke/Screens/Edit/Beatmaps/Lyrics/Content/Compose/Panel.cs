﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose;

public abstract partial class Panel : FocusedOverlayContainer
{
    private Sample? samplePopIn;
    private Sample? samplePopOut;

    private const float transition_length = 600;

    protected virtual string PopInSampleName => "UI/overlay-pop-in";
    protected virtual string PopOutSampleName => "UI/overlay-pop-out";

    private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
    private readonly Box background;
    private readonly FillFlowContainer fillFlowContainer;

    protected override bool BlockPositionalInput => false;

    protected Panel()
    {
        Padding = new MarginPadding(10);

        InternalChild = new Container
        {
            Masking = true,
            CornerRadius = 10,
            RelativeSizeAxes = Axes.Both,
            Children = new Drawable[]
            {
                background = new Box
                {
                    Name = "Background",
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0.6f,
                },
                new OsuScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = fillFlowContainer = new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(10),
                    },
                },
            },
        };
    }

    protected abstract IReadOnlyList<Drawable> CreateSections();

    [BackgroundDependencyLoader]
    private void load(ILyricEditorState state, LyricEditorColourProvider colourProvider, AudioManager audio)
    {
        bindableMode.BindTo(state.BindableMode);
        bindableMode.BindValueChanged(x =>
        {
            background.Colour = colourProvider.Background2(state.Mode);
        }, true);

        samplePopIn = audio.Samples.Get(PopInSampleName);
        samplePopOut = audio.Samples.Get(PopOutSampleName);
    }

    private PanelDirection direction;

    public PanelDirection Direction
    {
        get => direction;
        set
        {
            if (direction == value)
                return;

            direction = value;

            switch (direction)
            {
                case PanelDirection.Left:
                    Anchor = Anchor.TopLeft;
                    Origin = Anchor.TopLeft;
                    break;

                case PanelDirection.Right:
                    Anchor = Anchor.TopRight;
                    Origin = Anchor.TopRight;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(direction));
            }
        }
    }

    protected override void PopIn()
    {
        samplePopIn?.Play();

        this.FadeTo(1, transition_length, Easing.OutQuint);

        // should load the content after opened.
        fillFlowContainer.Children = CreateSections();
    }

    protected override void PopOut()
    {
        samplePopOut?.Play();

        this.FadeTo(0, transition_length, Easing.OutQuint).OnComplete(_ =>
        {
            // should clear the content if close.
            fillFlowContainer.Clear();
        });
    }
}
