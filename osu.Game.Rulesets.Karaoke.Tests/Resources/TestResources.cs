// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using NUnit.Framework;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.IO.Stores;
using osu.Game.Rulesets.Karaoke.Skinning.Legacy;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Tests.Resources
{
    public static class TestResources
    {
        public static DllResourceStore GetStore() => new(typeof(TestResources).Assembly);

        public static Stream OpenResource(string name) => GetStore().GetStream($"Resources/{name}");

        public static Stream OpenBeatmapResource(string name) => OpenResource($"Testing/Beatmaps/{name}.osu");

        public static Stream OpenSkinResource(string name) => OpenResource($"Testing/Skin/{name}.skin");

        public static Stream OpenLrcResource(string name) => OpenResource($"Testing/Lrc/{name}.lrc");

        public static string GetTestLrcForImport(string name)
        {
            var tempPath = Path.GetTempFileName() + ".lrc";

            using (var stream = OpenLrcResource(name))
            using (var newFile = File.Create(tempPath))
                stream.CopyTo(newFile);

            Assert.IsTrue(File.Exists(tempPath));
            return tempPath;
        }

        public static Stream OpenNicoKaraResource(string name) => OpenResource($"Testing/NicoKara/{name}.nkmproj");

        public static Stream OpenTrackResource(string name) => OpenResource($"Testing/Track/{name}.mp3");

        public static Track OpenTrackInfo(AudioManager audioManager, string name) => audioManager.GetTrackStore(GetStore()).Get($"Resources/Testing/Track/{name}.mp3");

        public static KaraokeLegacySkinTransformer GetKaraokeLegacySkinTransformer(string skinName)
        {
            var store = new NamespacedResourceStore<byte[]>(GetStore(), $"Resources/{skinName}");
            var rawSkin = new TestLegacySkin(new SkinInfo { Name = skinName }, store);
            var skinSource = new SkinProvidingContainer(rawSkin);
            return new KaraokeLegacySkinTransformer(skinSource, null);
        }

        internal class TestLegacySkin : LegacySkin
        {
            public TestLegacySkin(SkinInfo skin, IResourceStore<byte[]> storage)
                // Bypass LegacySkinResourceStore to avoid returning null for retrieving files due to bad skin info (SkinInfo.Files = null).
                : base(skin, storage, null, "skin.ini")
            {
            }
        }
    }
}
