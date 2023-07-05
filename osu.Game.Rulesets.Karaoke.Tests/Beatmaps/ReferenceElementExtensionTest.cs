// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps;

public class ReferenceElementExtensionTest
{
    private class TestClass : IHasReferenceElement, IHasPrimaryKey
    {
        public ElementId ID { get; } = ElementId.NewElementId();

        // todo: add the property
        // nullable property
        // list property
        // dictionary property
    }

    private static TestClass getTestClass()
    {
        // 需要有幾種組合:
        // non-null property, nullable property, list, dictionaty, to get the child object (需要同時包含一層或是兩層 recursive)
        // non-null property, nullable property, list, dictionaty, to get the child element id
        return new TestClass();
    }
}
