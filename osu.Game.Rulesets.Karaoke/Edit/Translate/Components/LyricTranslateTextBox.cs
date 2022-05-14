// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Input.Events;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate.Components
{
    public class LyricTranslateTextBox : OsuTextBox
    {
        private readonly IBindable<CultureInfo> currentLanguage = new Bindable<CultureInfo>();

        private readonly Lyric lyric;

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        [Resolved]
        private ILyricTranslateChangeHandler lyricTranslateChangeHandler { get; set; }

        [Resolved]
        private ITranslateInfoProvider translateInfoProvider { get; set; }

        public LyricTranslateTextBox(Lyric lyric)
        {
            this.lyric = lyric;

            currentLanguage.BindValueChanged(v =>
            {
                bool hasCultureInfo = v.NewValue != null;

                // disable and clear text box if contains no language in language list.
                Text = hasCultureInfo ? translateInfoProvider.GetLyricTranslate(lyric, v.NewValue) : null;
                ScheduleAfterChildren(() =>
                {
                    Current.Disabled = !hasCultureInfo;
                });
            }, true);

            OnCommit += (t, _) =>
            {
                string text = t.Text.Trim();

                var cultureInfo = currentLanguage.Value;
                if (cultureInfo == null)
                    return;

                lyricTranslateChangeHandler.UpdateTranslate(cultureInfo, text);
            };
        }

        protected override void OnFocus(FocusEvent e)
        {
            base.OnFocus(e);
            beatmap.SelectedHitObjects.Add(lyric);
        }

        protected override void OnFocusLost(FocusLostEvent e)
        {
            base.OnFocusLost(e);
            Schedule(() =>
            {
                // should remove lyric until commit finished.
                beatmap.SelectedHitObjects.Remove(lyric);
            });
        }

        [BackgroundDependencyLoader]
        private void load(IBindable<CultureInfo> currentLanguage)
        {
            this.currentLanguage.BindTo(currentLanguage);
        }
    }
}
