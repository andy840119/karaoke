// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Beatmaps;

/// <summary>
/// Extension that able to get all the <see cref="ElementId"/> from the <see cref="IHasReferenceElement"/> by different way.
/// </summary>
public static class ReferenceElementExtension
{
    /// <summary>
    /// Get all collections that contains the <see cref="ElementId"/> and who referenced it.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static IEnumerable<KeyValuePair<ElementId, IHasReferenceElement>> GetReferenceCollection(this IHasReferenceElement element)
    {
        // todo: should deal with recursive.
        return new List<KeyValuePair<ElementId, IHasReferenceElement>>();
    }

    /// <summary>
    /// Get the <see cref="IDictionary{TKey,TValue}"/> that the <see cref="ElementId"/> is referenced by which <see cref="IHasReferenceElement"/>
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static IDictionary<ElementId, IHasReferenceElement[]> GetReferenceDictionary(this IHasReferenceElement element)
    {
        return new Dictionary<ElementId, IHasReferenceElement[]>();
    }

    /// <summary>
    /// Get all the reference search by <see cref="ElementId"/>
    /// </summary>
    /// <param name="element"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static IEnumerable<IHasReferenceElement> GetAllReferenceObject(this IHasReferenceElement element, ElementId id)
    {
        return Array.Empty<IHasReferenceElement>();
    }

    /// <summary>
    /// Get all child <see cref="IHasReferenceElement"/> by parent <see cref="IHasReferenceElement"/>
    /// Will check all the public properties from the <see cref="IHasReferenceElement"/>
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static IEnumerable<IHasReferenceElement> GetAllChildReferenceElement(this IHasReferenceElement element)
    {
        // todo: need to scan the property, list view and dictionary.
        return Array.Empty<IHasReferenceElement>();
    }

    /// <summary>
    /// Get all available referenced <see cref="ElementId"/> by parent <see cref="IHasReferenceElement"/>
    /// Will check all the public properties from the <see cref="IHasReferenceElement"/>
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static IEnumerable<ElementId> GetAllReferencedElementId(this IHasReferenceElement element)
    {
        // todo: need to scan the property, list view and dictionary that contains the element id.
        return Array.Empty<ElementId>();
    }
}
