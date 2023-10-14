// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.IO.Serialization;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning;

/// <summary>
/// It's the skin that designed for reading the resource file and parse into the understandable format form beatmap skin.
/// </summary>
public class KaraokeBeatmapSkin : KaraokeSkin
{
    public readonly IDictionary<ElementType, IList<IKaraokeSkinElement>> Elements = new Dictionary<ElementType, IList<IKaraokeSkinElement>>();

    public KaraokeBeatmapSkin(SkinInfo skin, IStorageResourceProvider? resources, IResourceStore<byte[]>? storage = null)
        : base(skin, resources, storage)
    {
        SkinInfo.PerformRead(s =>
        {
            var globalSetting = SkinJsonSerializableExtensions.CreateSkinElementGlobalSettings();

            // we may want to move this to some kind of async operation in the future.
            foreach (ElementType skinnableTarget in Enum.GetValues<ElementType>())
            {
                string filename = $"{getFileNameByType(skinnableTarget)}.json";

                try
                {
                    Elements.Add(skinnableTarget, new List<IKaraokeSkinElement>());

                    string? jsonContent = GetElementStringContentFromSkinInfo(s, filename);
                    if (string.IsNullOrEmpty(jsonContent))
                        return;

                    var deserializedContent = JsonConvert.DeserializeObject<IKaraokeSkinElement[]>(jsonContent, globalSetting);

                    if (deserializedContent == null)
                        continue;

                    Elements[skinnableTarget] = deserializedContent;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Failed to load skin element.");
                }
            }

            static string getFileNameByType(ElementType elementType)
                => elementType switch
                {
                    ElementType.LyricFontInfo => "lyric-font-infos",
                    ElementType.LyricStyle => "lyric-styles",
                    ElementType.NoteStyle => "note-styles",
                    _ => throw new InvalidEnumArgumentException(nameof(elementType)),
                };
        });
    }

    public override IBindable<TValue>? GetConfig<TLookup, TValue>(TLookup lookup)
    {
        switch (lookup)
        {
            // get the target element by hit object.
            case KaraokeHitObject hitObject:
            {
                var type = typeof(TValue);
                var element = GetElementByHitObjectAndElementType(hitObject, type);
                return SkinUtils.As<TValue>(new Bindable<TValue>((TValue)element!));
            }

            // in some cases, we still need to get target of element by type and id.
            // e.d: get list of layout in the skin manager.
            case KaraokeSkinLookup skinLookup:
            {
                var type = skinLookup.Type;
                int lookupNumber = skinLookup.Lookup;
                if (lookupNumber < 0)
                    return base.GetConfig<KaraokeSkinLookup, TValue>(skinLookup);

                var element = Elements[type].FirstOrDefault(x => x.ID == lookupNumber);
                return SkinUtils.As<TValue>(new Bindable<TValue>((TValue)element!));
            }

            // Lookup list of name by type
            case KaraokeIndexLookup indexLookup:
                return indexLookup switch
                {
                    KaraokeIndexLookup.Style => SkinUtils.As<TValue>(getSelectionFromElementType(ElementType.LyricStyle)),
                    KaraokeIndexLookup.Note => SkinUtils.As<TValue>(getSelectionFromElementType(ElementType.NoteStyle)),
                    _ => throw new InvalidEnumArgumentException(nameof(indexLookup)),
                };

            case KaraokeSkinConfigurationLookup skinConfigurationLookup:
                return base.GetConfig<KaraokeSkinConfigurationLookup, TValue>(skinConfigurationLookup);
        }

        return null;

        Bindable<IDictionary<int, string>> getSelectionFromElementType(ElementType elementType)
            => new(Elements[elementType].ToDictionary(k => k.ID, k => k.Name));
    }
}
