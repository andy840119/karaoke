// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Beatmaps;

/// <summary>
/// Mark the object in the karaoke beatmap / stage that has referenced <see cref="ElementId"/>.
/// All the object that contains referenced <see cref="ElementId"/> or contains <see cref="IHasReferenceElement"/> in the property should inherit this interface.
/// </summary>
public interface IHasReferenceElement
{
}
