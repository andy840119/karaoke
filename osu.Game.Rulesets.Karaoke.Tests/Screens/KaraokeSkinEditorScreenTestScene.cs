// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using System.Reflection;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.IO;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Screens.Skin;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Skinning;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens
{
    public abstract class KaraokeSkinEditorScreenTestScene<T> : EditorClockTestScene where T : KaraokeSkinEditorScreen
    {
        [Cached]
        private readonly OverlayColourProvider colourProvider = new(OverlayColourScheme.Pink);

        private readonly KaraokeBeatmapSkin karaokeSkin = new TestKaraokeBeatmapSkin();

        protected override void LoadComplete()
        {
            Child = new SkinProvidingContainer(karaokeSkin)
            {
                RelativeSizeAxes = Axes.Both,
                Child = CreateEditorScreen(karaokeSkin).With(x =>
                {
                    x.State.Value = Visibility.Visible;
                })
            };
        }

        protected abstract T CreateEditorScreen(KaraokeSkin karaokeSkin);

        protected class TestKaraokeBeatmapSkin : KaraokeBeatmapSkin
        {
            public TestKaraokeBeatmapSkin()
                : base(new SkinInfo(), null)
            {
                // TODO : need a better way to load resource
                var assembly = Assembly.GetExecutingAssembly();
                const string resource_name = @"osu.Game.Rulesets.Karaoke.Tests.Resources.Testing.Skin.default.skin";

                using (var stream = assembly.GetManifestResourceStream(resource_name))
                using (var reader = new LineBufferedReader(stream))
                {
                    var karaokeSkin = new KaraokeSkinDecoder().Decode(reader);

                    // Default values
                    DefaultElement[ElementType.LyricConfig] = karaokeSkin.DefaultLyricConfig;
                    DefaultElement[ElementType.LyricStyle] = karaokeSkin.DefaultLyricStyle;
                    DefaultElement[ElementType.NoteStyle] = karaokeSkin.DefaultNoteStyle;

                    // Create bindable
                    foreach (LyricLayout t in karaokeSkin.Layouts)
                        Elements[ElementType.LyricLayout].Add(t);

                    foreach (LyricStyle t in karaokeSkin.LyricStyles)
                        Elements[ElementType.LyricStyle].Add(t);

                    foreach (NoteStyle t in karaokeSkin.NoteStyles)
                        Elements[ElementType.NoteStyle].Add(t);

                    // before apply new logic, add fake lyric layout for prevent test failed.
                    Elements[ElementType.LyricConfig].Add(LyricConfig.DEFAULT);

                    // Create lookups
                    BindableFontsLookup.Value = karaokeSkin.LyricStyles.ToDictionary(k => karaokeSkin.LyricStyles.IndexOf(k), y => y.Name);
                    BindableLayoutsLookup.Value = karaokeSkin.Layouts.ToDictionary(k => karaokeSkin.Layouts.IndexOf(k), y => y.Name);
                    BindableNotesLookup.Value = karaokeSkin.NoteStyles.ToDictionary(k => karaokeSkin.NoteStyles.IndexOf(k), y => y.Name);
                }
            }
        }
    }
}
