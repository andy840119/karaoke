// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Skinning.Groups;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters
{
    public class KaraokeSkinGroupConvertorTest : BaseSingleConverterTest<KaraokeSkinGroupConvertor>
    {
        [Test]
        public void TestGroupBySingerIdsSerializer()
        {
            var group = new GroupBySingerIds
            {
                ID = 123,
                Name = "Singer 1 and 2",
                SingerIds = new[] { 1, 2 }
            };

            const string expected = "{\"$type\":\"GroupBySingerIds\",\"singer_ids\":[1,2],\"id\":123,\"name\":\"Singer 1 and 2\"}";
            string actual = JsonConvert.SerializeObject(group, CreateSettings());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestGroupBySingerIdsDeserializer()
        {
            const string json = "{\"$type\":\"GroupBySingerIds\",\"singer_ids\":[1,2],\"id\":123,\"name\":\"Singer 1 and 2\"}";

            var expected = new GroupBySingerIds
            {
                ID = 123,
                Name = "Singer 1 and 2",
                SingerIds = new[] { 1, 2 }
            };
            var actual = JsonConvert.DeserializeObject<IGroup>(json, CreateSettings()) as GroupBySingerIds;
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.ID, actual.ID);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.SingerIds, actual.SingerIds);
        }

        [Test]
        public void TestGroupBySingerNumberSerializer()
        {
            var group = new GroupBySingerNumber
            {
                ID = 123,
                Name = "Two singers",
                SingerNumber = 2
            };

            const string expected = "{\"$type\":\"GroupBySingerNumber\",\"singer_number\":2,\"id\":123,\"name\":\"Two singers\"}";
            string actual = JsonConvert.SerializeObject(group, CreateSettings());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestGroupBySingerNumberDeserializer()
        {
            const string json = "{\"$type\":\"GroupBySingerNumber\",\"singer_number\":2,\"id\":123,\"name\":\"Two singers\"}";

            var expected = new GroupBySingerNumber
            {
                ID = 123,
                Name = "Two singers",
                SingerNumber = 2
            };
            var actual = JsonConvert.DeserializeObject<IGroup>(json, CreateSettings()) as GroupBySingerNumber;
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.ID, actual.ID);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.SingerNumber, actual.SingerNumber);
        }

        [Test]
        public void TestGroupByLyricIdsSerializer()
        {
            var group = new GroupByLyricIds
            {
                ID = 123,
                Name = "Lyric 1 and 2",
                LyricIds = new[] { 1, 2 }
            };

            const string expected = "{\"$type\":\"GroupByLyricIds\",\"lyric_ids\":[1,2],\"id\":123,\"name\":\"Lyric 1 and 2\"}";
            string actual = JsonConvert.SerializeObject(group, CreateSettings());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestGroupByLyricIdsDeserializer()
        {
            const string json = "{\"$type\":\"GroupByLyricIds\",\"lyric_ids\":[1,2],\"id\":123,\"name\":\"Lyric 1 and 2\"}";

            var expected = new GroupByLyricIds
            {
                ID = 123,
                Name = "Lyric 1 and 2",
                LyricIds = new[] { 1, 2 }
            };
            var actual = JsonConvert.DeserializeObject<IGroup>(json, CreateSettings()) as GroupByLyricIds;
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.ID, actual.ID);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.LyricIds, actual.LyricIds);
        }
    }
}
