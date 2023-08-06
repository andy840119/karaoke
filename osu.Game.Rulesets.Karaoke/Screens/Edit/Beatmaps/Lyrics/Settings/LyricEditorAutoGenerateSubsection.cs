﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public abstract partial class LyricEditorAutoGenerateSubsection<TChangeHandler> : AutoGenerateSubsection
    where TChangeHandler : class, ILyricPropertyAutoGenerateChangeHandler
{
    protected override EditorSectionButton CreateGenerateButton()
        => new AutoGenerateButton();

    protected sealed override DescriptionTextFlowContainer CreateDescriptionTextFlowContainer()
        => new LyricEditorDescriptionTextFlowContainer();

    private partial class AutoGenerateButton : SelectLyricButton
    {
        [Resolved]
        private TChangeHandler changeHandler { get; set; } = null!;

        protected override LocalisableString StandardText => "Generate";

        protected override LocalisableString SelectingText => "Cancel generate";

        protected override IDictionary<Lyric, LocalisableString> GetDisableSelectingLyrics()
        {
            return changeHandler.GetGeneratorNotSupportedLyrics();
        }

        protected override void Apply()
        {
            changeHandler.AutoGenerate();
        }
    }
}

public abstract partial class LyricEditorAutoGenerateSubsection<TChangeHandler, TType> : AutoGenerateSubsection
    where TChangeHandler : class, ILyricPropertyEnumAutoGenerateChangeHandler<TType>
    where TType : Enum
{
    private readonly TType type;

    protected LyricEditorAutoGenerateSubsection(TType type)
    {
        this.type = type;
    }

    protected override EditorSectionButton CreateGenerateButton()
        => new AutoGenerateButton(type);

    protected sealed override DescriptionTextFlowContainer CreateDescriptionTextFlowContainer()
        => new LyricEditorDescriptionTextFlowContainer();

    private partial class AutoGenerateButton : SelectLyricButton
    {
        [Resolved]
        private TChangeHandler changeHandler { get; set; } = null!;

        protected override LocalisableString StandardText => "Generate";

        protected override LocalisableString SelectingText => "Cancel generate";

        private readonly TType type;

        public AutoGenerateButton(TType type)
        {
            this.type = type;
        }

        protected override IDictionary<Lyric, LocalisableString> GetDisableSelectingLyrics()
        {
            return changeHandler.GetGeneratorNotSupportedLyrics(type);
        }

        protected override void Apply()
        {
            changeHandler.AutoGenerate(type);
        }
    }
}
