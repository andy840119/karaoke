﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose.Toolbar;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose.Toolbar.Carets;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose.Toolbar.Panels;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose.Toolbar.Playback;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose.Toolbar.View;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose;

public partial class SpecialActionToolbar : CompositeDrawable
{
    public const int HEIGHT = 26;
    public const int PADDING = 2;
    public const int ICON_SIZE = HEIGHT - PADDING * 2;

    public const int SPACING = 5;

    private readonly IBindable<EditorModeWithEditStep> bindableModeWithEditStep = new Bindable<EditorModeWithEditStep>();

    private readonly Box background;

    private readonly FillFlowContainer buttonContainer;

    public SpecialActionToolbar()
    {
        AutoSizeAxes = Axes.Both;

        InternalChildren = new Drawable[]
        {
            background = new Box
            {
                RelativeSizeAxes = Axes.Both,
            },
            buttonContainer = new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Padding = new MarginPadding(5),
                Spacing = new Vector2(SPACING),
            },
        };
    }

    [BackgroundDependencyLoader]
    private void load(ILyricEditorState state, LyricEditorColourProvider colourProvider)
    {
        bindableModeWithEditStep.BindTo(state.BindableModeWithEditStep);
        bindableModeWithEditStep.BindValueChanged(e =>
        {
            // Note: add the schedule because will have the "The collection's state is no longer correct." error if not add this.
            Schedule(reGenerateButtons);

            if (ValueChangedEventUtils.EditModeChanged(e) || !IsLoaded)
                background.Colour = colourProvider.Background2(state.Mode);
        }, true);
    }

    private void reGenerateButtons()
    {
        buttonContainer.Clear();

        buttonContainer.AddRange(createAdjustLyricSizeItem());

        buttonContainer.Add(new Separator());

        buttonContainer.AddRange(createPanelItems());

        buttonContainer.Add(new Separator());

        buttonContainer.AddRange(createSwitchLyricItem());

        buttonContainer.Add(new Separator());

        var modeWithEditStep = bindableModeWithEditStep.Value;
        buttonContainer.AddRange(createItemForEditMode(modeWithEditStep));
    }

    private static IEnumerable<Drawable> createAdjustLyricSizeItem() => new Drawable[]
    {
        new AdjustFontSizeButton(),
    };

    private static IEnumerable<Drawable> createPanelItems() => new Drawable[]
    {
        new TogglePropertyPanelButton(),
        new ToggleInvalidInfoPanelButton(),
    };

    private static IEnumerable<Drawable> createSwitchLyricItem() => new Drawable[]
    {
        new MoveToPreviousLyricButton(),
        new MoveToNextLyricButton(),
    };

    private static IEnumerable<Drawable> createItemForEditMode(EditorModeWithEditStep editorModeWithEditStep)
    {
        return editorModeWithEditStep.Mode switch
        {
            LyricEditorMode.View => Array.Empty<Drawable>(),
            LyricEditorMode.EditText => createItemsForTextEditStep(editorModeWithEditStep.GetEditStep<TextEditStep>()),
            LyricEditorMode.EditReferenceLyric => Array.Empty<Drawable>(),
            LyricEditorMode.EditLanguage => Array.Empty<Drawable>(),
            LyricEditorMode.EditRuby => Array.Empty<Drawable>(),
            LyricEditorMode.EditTimeTag => createItemsForTimeTagEditStep(editorModeWithEditStep.GetEditStep<TimeTagEditStep>()),
            LyricEditorMode.EditRomanisation => Array.Empty<Drawable>(),
            LyricEditorMode.EditNote => createItemsForNoteEditStep(editorModeWithEditStep.GetEditStep<NoteEditStep>()),
            LyricEditorMode.EditSinger => Array.Empty<Drawable>(),
            _ => throw new ArgumentOutOfRangeException(),
        };

        static IEnumerable<Drawable> createItemsForTextEditStep(TextEditStep textEditMode)
        {
            switch (textEditMode)
            {
                case TextEditStep.Typing:
                case TextEditStep.Split:
                    return new Drawable[]
                    {
                        new MoveToFirstIndexButton(),
                        new MoveToPreviousIndexButton(),
                        new MoveToNextIndexButton(),
                        new MoveToLastIndexButton(),
                    };

                case TextEditStep.Verify:
                    return Array.Empty<Drawable>();

                default:
                    throw new ArgumentOutOfRangeException(nameof(textEditMode));
            }
        }

        static IEnumerable<Drawable> createItemsForTimeTagEditStep(TimeTagEditStep timeTagEditMode) =>
            timeTagEditMode switch
            {
                TimeTagEditStep.Create => new Drawable[]
                {
                    new MoveToFirstIndexButton(),
                    new MoveToPreviousIndexButton(),
                    new MoveToNextIndexButton(),
                    new MoveToLastIndexButton(),
                },
                TimeTagEditStep.Recording => new Drawable[]
                {
                    new PlaybackSwitchButton(),
                    new Separator(),
                    new MoveToFirstIndexButton(),
                    new MoveToPreviousIndexButton(),
                    new MoveToNextIndexButton(),
                    new MoveToLastIndexButton(),
                },
                TimeTagEditStep.Adjust => new Drawable[]
                {
                    new PlaybackSwitchButton(),
                },
                _ => throw new ArgumentOutOfRangeException(nameof(timeTagEditMode), timeTagEditMode, null),
            };

        static IEnumerable<Drawable> createItemsForNoteEditStep(NoteEditStep noteEditMode) =>
            noteEditMode switch
            {
                NoteEditStep.Generate => Array.Empty<Drawable>(),
                NoteEditStep.Edit => Array.Empty<Drawable>(),
                NoteEditStep.Verify => Array.Empty<Drawable>(),
                _ => throw new ArgumentOutOfRangeException(nameof(noteEditMode), noteEditMode, null),
            };
    }
}
