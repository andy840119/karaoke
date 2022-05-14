// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using NUnit.Framework;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Game.Database;
using osu.Game.IO;

namespace osu.Game.Rulesets.Karaoke.Tests.Resources
{
    public static class TestResources
    {
        public static DllResourceStore GetStore()
        {
            return new(typeof(TestResources).Assembly);
        }

        public static Stream OpenResource(string name)
        {
            return GetStore().GetStream($"Resources/{name}");
        }

        public static Stream OpenBeatmapResource(string name)
        {
            return OpenResource($"Testing/Beatmaps/{name}.osu");
        }

        public static Stream OpenSkinResource(string name)
        {
            return OpenResource($"Testing/Skin/{name}.skin");
        }

        public static Stream OpenLrcResource(string name)
        {
            return OpenResource($"Testing/Lrc/{name}.lrc");
        }

        public static string GetTestLrcForImport(string name)
        {
            string tempPath = Path.GetTempFileName() + ".lrc";

            using (var stream = OpenLrcResource(name))
            using (var newFile = File.Create(tempPath))
                stream.CopyTo(newFile);

            Assert.IsTrue(File.Exists(tempPath));
            return tempPath;
        }

        public static Stream OpenNicoKaraResource(string name)
        {
            return OpenResource($"Testing/NicoKara/{name}.nkmproj");
        }

        public static Stream OpenTrackResource(string name)
        {
            return OpenResource($"Testing/Track/{name}.mp3");
        }

        public static Track OpenTrackInfo(AudioManager audioManager, string name)
        {
            return audioManager.GetTrackStore(GetStore()).Get($"Resources/Testing/Track/{name}.mp3");
        }

        public static IStorageResourceProvider CreateSkinStorageResourceProvider(string skinName = "special-skin")
        {
            return new TestStorageResourceProvider(skinName);
        }

        private class TestStorageResourceProvider : IStorageResourceProvider
        {
            public AudioManager AudioManager => null;
            public IResourceStore<byte[]> Files { get; }
            public IResourceStore<byte[]> Resources { get; }
            public RealmAccess RealmAccess => null;

            public TestStorageResourceProvider(string skinName)
            {
                Files = Resources = new NamespacedResourceStore<byte[]>(new DllResourceStore(GetType().Assembly), $"Resources/{skinName}");
            }

            public IResourceStore<TextureUpload> CreateTextureLoaderStore(IResourceStore<byte[]> underlyingStore)
            {
                return null;
            }
        }
    }
}
