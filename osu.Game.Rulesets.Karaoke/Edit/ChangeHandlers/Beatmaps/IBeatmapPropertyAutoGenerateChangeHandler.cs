// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

public interface IBeatmapPropertyTypeAutoGenerateChangeHandler<in TType> : ITypeAutoGenerateChangeHandler<TType> where TType : notnull
{
    LocalisableString? GetGeneratorNotSupportedMessage<T>() where T : TType;
}

public interface IBeatmapPropertyAutoGenerateChangeHandler : IAutoGenerateChangeHandler
{
    LocalisableString? GetGeneratorNotSupportedMessage();
}
