﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using osu.Framework.Graphics.Sprites;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Helper
{
    public static class TestCaseTagHelper
    {
        /// <summary>
        ///     Process test case ruby string format into <see cref="RubyTag" />
        /// </summary>
        /// <example>
        ///     [0,3]:ruby
        /// </example>
        /// <param name="str">Ruby tag string format</param>
        /// <returns><see cref="RubyTag" />Ruby tag object</returns>
        public static RubyTag ParseRubyTag(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new RubyTag();

            var regex = new Regex("(?<start>[-0-9]+),(?<end>[-0-9]+)]:(?<ruby>.*$)");
            var result = regex.Match(str);
            if (!result.Success)
                throw new RegexMatchTimeoutException(nameof(str));

            return new RubyTag
            {
                StartIndex = result.GetGroupValue<int>("start"),
                EndIndex = result.GetGroupValue<int>("end"),
                Text = result.GetGroupValue<string>("ruby")
            };
        }

        /// <summary>
        ///     Process test case romaji string format into <see cref="RomajiTag" />
        /// </summary>
        /// <example>
        ///     [0,3]:romaji
        /// </example>
        /// <param name="str">Romaji tag string format</param>
        /// <returns><see cref="RomajiTag" />Romaji tag object</returns>
        public static RomajiTag ParseRomajiTag(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new RomajiTag();

            var regex = new Regex("(?<start>[-0-9]+),(?<end>[-0-9]+)]:(?<romaji>.*$)");
            var result = regex.Match(str);
            if (!result.Success)
                throw new RegexMatchTimeoutException(nameof(str));

            return new RomajiTag
            {
                StartIndex = result.GetGroupValue<int>("start"),
                EndIndex = result.GetGroupValue<int>("end"),
                Text = result.GetGroupValue<string>("romaji")
            };
        }

        /// <summary>
        ///     Process test case time tag string format into <see cref="TimeTag" />
        /// </summary>
        /// <example>
        ///     [0,start]:1000
        /// </example>
        /// <param name="str">Time tag string format</param>
        /// <returns><see cref="TimeTag" />Time tag object</returns>
        public static TimeTag ParseTimeTag(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new TimeTag(new TextIndex());

            var regex = new Regex("(?<index>[-0-9]+),(?<state>start|end)]:(?<time>[-0-9]+|s*|)");
            var result = regex.Match(str);
            if (!result.Success)
                throw new RegexMatchTimeoutException(nameof(str));

            int index = result.GetGroupValue<int>("index");
            var state = result.GetGroupValue<string>("state") == "start" ? TextIndex.IndexState.Start : TextIndex.IndexState.End;
            int? time = result.GetGroupValue<int?>("time");

            return new TimeTag(new TextIndex(index, state), time);
        }

        /// <summary>
        ///     Process test case text index string format into <see cref="TextIndex" />
        /// </summary>
        /// <example>
        ///     [0,start]
        /// </example>
        /// <param name="str">Text tag string format</param>
        /// <returns><see cref="TimeTag" />Text tag object</returns>
        public static TextIndex ParseTextIndex(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new TextIndex();

            var regex = new Regex("(?<index>[-0-9]+),(?<state>start|end)]");
            var result = regex.Match(str);
            if (!result.Success)
                throw new RegexMatchTimeoutException(nameof(str));

            int index = result.GetGroupValue<int>("index");
            var state = result.GetGroupValue<string>("state") == "start" ? TextIndex.IndexState.Start : TextIndex.IndexState.End;

            return new TextIndex(index, state);
        }

        /// <summary>
        ///     Process test case lyric string format into <see cref="Lyric" />
        /// </summary>
        /// <example>
        ///     [1000,3000]:karaoke
        /// </example>
        /// <param name="str">Lyric string format</param>
        /// <returns><see cref="Lyric" />Lyric object</returns>
        public static Lyric ParseLyric(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new Lyric();

            var regex = new Regex("(?<startTime>[-0-9]+),(?<endTime>[-0-9]+)]:(?<lyric>.*$)");
            var result = regex.Match(str);
            if (!result.Success)
                throw new RegexMatchTimeoutException(nameof(str));

            double startTime = result.GetGroupValue<double>("startTime");
            double endTime = result.GetGroupValue<double>("endTime");
            string text = result.GetGroupValue<string>("lyric");

            return new Lyric
            {
                StartTime = startTime,
                Duration = endTime - startTime,
                Text = text,
                TimeTags = new[]
                {
                    new TimeTag(new TextIndex(0), startTime),
                    new TimeTag(new TextIndex((text?.Length ?? 0) - 1, TextIndex.IndexState.End), endTime)
                }
            };
        }

        /// <summary>
        ///     Process test case lyric string format into <see cref="Lyric" />
        /// </summary>
        /// <example>
        ///     "[00:01.00]か[00:02.00]ら[00:03.00]お[00:04.00]け[00:05.00]"
        /// </example>
        /// <param name="str">Lyric string format</param>
        /// <returns><see cref="Lyric" />Lyric object</returns>
        public static Lyric ParseLyricWithTimeTag(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new Lyric();

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            using (var reader = new LineBufferedReader(stream))
            {
                // Create stream
                writer.Write(str);
                writer.Flush();
                stream.Position = 0;

                // Create karaoke note decoder
                var decoder = new LrcDecoder();
                return decoder.Decode(reader).HitObjects.OfType<Lyric>().FirstOrDefault();
            }
        }

        /// <summary>
        ///     Process test case singer string format into <see cref="Singer" />
        /// </summary>
        /// <example>
        ///     [0]name:singer001
        ///     [0]romaji:singer001
        ///     [0]eg:singer001
        /// </example>
        /// <param name="str">Singer string format</param>
        /// <returns><see cref="Singer" />sSinger object</returns>
        public static Singer ParseSinger(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new Singer(0);

            var regex = new Regex("(?<id>[-0-9]+)]");
            var result = regex.Match(str);
            if (!result.Success)
                throw new RegexMatchTimeoutException(nameof(str));

            // todo : implementation
            int id = result.GetGroupValue<int>("id");

            return new Singer(id);
        }

        public static RubyTag[] ParseRubyTags(IEnumerable<string> strings)
        {
            return strings?.Select(ParseRubyTag).ToArray();
        }

        public static RomajiTag[] ParseRomajiTags(IEnumerable<string> strings)
        {
            return strings?.Select(ParseRomajiTag).ToArray();
        }

        public static TimeTag[] ParseTimeTags(IEnumerable<string> strings)
        {
            return strings?.Select(ParseTimeTag).ToArray();
        }

        public static Lyric[] ParseLyrics(IEnumerable<string> strings)
        {
            return strings?.Select(ParseLyric).ToArray();
        }

        public static Singer[] ParseSingers(IEnumerable<string> strings)
        {
            return strings?.Select(ParseSinger).ToArray();
        }
    }
}
