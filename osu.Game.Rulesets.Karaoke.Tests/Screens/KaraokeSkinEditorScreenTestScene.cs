// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Screens.Skin;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens
{
    public abstract class KaraokeSkinEditorScreenTestScene<T> : EditorClockTestScene where T : KaraokeSkinEditorScreen
    {
        [Test]
        public void TestKaraoke() => runForRuleset(new KaraokeRuleset().RulesetInfo);

        private void runForRuleset(RulesetInfo rulesetInfo)
        {
            AddStep("create screen", () =>
            {
                Child = CreateEditorScreen().With(x =>
                {
                    x.State.Value = Visibility.Visible;
                });
            });
        }

        protected abstract T CreateEditorScreen();
    }
}