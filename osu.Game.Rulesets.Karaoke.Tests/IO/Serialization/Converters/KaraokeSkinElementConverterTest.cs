// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using osu.Framework.IO.Serialization;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters;

public class KaraokeSkinElementConverterTest : BaseSingleConverterTest<KaraokeSkinElementConverter>
{
    protected override IEnumerable<JsonConverter> CreateExtraConverts()
    {
        yield return new ColourConverter();
        yield return new Vector2Converter();
        yield return new ShaderConverter();
        yield return new FontUsageConverter();
    }

    [Test]
    public void TestNoteStyleSerializer()
    {
        var lyricConfig = NoteStyle.CreateDefault();

        const string expected = "{\"$type\":1,\"name\":\"Default\",\"note_color\":\"#44AADD\",\"blink_color\":\"#FF66AA\",\"text_color\":\"#FFFFFF\",\"bold_text\":true}";
        string actual = JsonConvert.SerializeObject(lyricConfig, CreateSettings());
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestNoteStyleDeserializer()
    {
        const string json = "{\"$type\":1,\"name\":\"Default\",\"note_color\":\"#44AADD\",\"blink_color\":\"#FF66AA\",\"text_color\":\"#FFFFFF\",\"bold_text\":true}";

        var expected = NoteStyle.CreateDefault();
        var actual = (NoteStyle)JsonConvert.DeserializeObject<IKaraokeSkinElement>(json, CreateSettings())!;
        ObjectAssert.ArePropertyEqual(expected, actual);
    }
}
