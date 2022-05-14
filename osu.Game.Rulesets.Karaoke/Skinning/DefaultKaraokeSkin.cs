// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using JetBrains.Annotations;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public class DefaultKaraokeSkin : KaraokeSkin
    {
        internal static readonly Guid DEFAULT_SKIN = new("FEC5A291-5709-11EC-9F10-0800200C9A66");

        public DefaultKaraokeSkin(IStorageResourceProvider resources)
            : this(CreateInfo(), resources)
        {
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        public DefaultKaraokeSkin(SkinInfo skin, IStorageResourceProvider resources)
            : base(skin, resources)
        {
            DefaultElement[ElementType.LyricConfig] = LyricConfig.CreateDefault();
            DefaultElement[ElementType.LyricStyle] = LyricStyle.CreateDefault();
            DefaultElement[ElementType.NoteStyle] = NoteStyle.CreateDefault();
        }

        public static SkinInfo CreateInfo()
        {
            return new()
            {
                ID = DEFAULT_SKIN,
                Name = "karaoke! (default skin)",
                Creator = "team karaoke!",
                Protected = true,
                InstantiationInfo = typeof(DefaultKaraokeSkin).GetInvariantInstantiationInfo()
            };
        }
    }
}
