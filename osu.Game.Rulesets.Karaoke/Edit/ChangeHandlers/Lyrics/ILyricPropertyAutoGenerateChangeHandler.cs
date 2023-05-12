// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;

public interface ILyricPropertyAutoGenerateChangeHandler : IAutoGenerateChangeHandler
{
    IDictionary<Lyric, LocalisableString> GetGeneratorNotSupportedLyrics();
}