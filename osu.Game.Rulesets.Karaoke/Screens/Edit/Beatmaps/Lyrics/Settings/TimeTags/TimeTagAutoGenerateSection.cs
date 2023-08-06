﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.TimeTags;

public partial class TimeTagAutoGenerateSection : AutoGenerateSection
{
    protected override AutoGenerateSubsection CreateAutoGenerateSubsection()
        => new TimeTageAutoGenerateSubsection();

    private partial class TimeTageAutoGenerateSubsection : LyricEditorAutoGenerateSubsection<ILyricTimeTagsChangeHandler, TimeTagGeneratorType>
    {
        public TimeTageAutoGenerateSubsection()
            : base(TimeTagGeneratorType.TimeTag)
        {
        }

        private const string language_mode = "LANGUAGE_MODE";

        protected override DescriptionFormat CreateInvalidDescriptionFormat()
            => new()
            {
                Text = $"Seems some lyric missing language, go to [{DescriptionFormat.LINK_KEY_ACTION}]({language_mode}) to fill the language.",
                Actions = new Dictionary<string, IDescriptionAction>
                {
                    {
                        language_mode, new SwitchModeDescriptionAction
                        {
                            Text = "edit language mode",
                            Mode = LyricEditorMode.Language,
                        }
                    },
                },
            };

        protected override ConfigButton CreateConfigButton()
            => new TimeTagAutoGenerateConfigButton();

        protected partial class TimeTagAutoGenerateConfigButton : MultiConfigButton
        {
            protected override IEnumerable<KaraokeRulesetEditGeneratorSetting> AvailableSettings => new[]
            {
                KaraokeRulesetEditGeneratorSetting.JaTimeTagGeneratorConfig,
                KaraokeRulesetEditGeneratorSetting.ZhTimeTagGeneratorConfig,
            };

            protected override string GetDisplayName(KaraokeRulesetEditGeneratorSetting setting) =>
                setting switch
                {
                    KaraokeRulesetEditGeneratorSetting.JaTimeTagGeneratorConfig => "Japanese",
                    KaraokeRulesetEditGeneratorSetting.ZhTimeTagGeneratorConfig => "Chinese",
                    _ => throw new ArgumentOutOfRangeException(nameof(setting)),
                };
        }
    }
}
