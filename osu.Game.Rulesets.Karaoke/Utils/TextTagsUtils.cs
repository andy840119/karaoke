﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects.Types;
using System.Collections.Generic;
using System.Linq;
using System;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class TextTagsUtils
    {
        public static T[] Sort<T>(T[] textTags, Sorting sorting = Sorting.Asc) where T : ITextTag
        {
            switch (sorting)
            {
                case Sorting.Asc:
                    return textTags?.OrderBy(x => x.StartIndex).ThenBy(x => x.EndIndex).ToArray();

                case Sorting.Desc:
                    return textTags?.OrderByDescending(x => x.EndIndex).ThenByDescending(x => x.StartIndex).ToArray();

                default:
                    throw new ArgumentOutOfRangeException(nameof(sorting));
            }
        }

        public static T[] FindInvalid<T>(T[] textTags, string lyric, Sorting sorting = Sorting.Asc) where T : ITextTag
        {
            // check is null or empty
            if (textTags == null || textTags.Length == 0)
                return new T[] { };

            // todo : need to make suure is need to sort in here?
            var sortedTextTags = Sort(textTags, sorting);

            var invalidList = new List<T>();

            // check invalid range
            invalidList.AddRange(sortedTextTags.Where(x => x.StartIndex < 0 || x.EndIndex > lyric.Length));

            // check end is less or equal to start index
            invalidList.AddRange(sortedTextTags.Where(x => x.EndIndex <= x.StartIndex));

            // find other is smaller or bigger
            foreach (var textTag in sortedTextTags)
            {
                if (invalidList.Contains(textTag))
                    continue;

                var checkTags = sortedTextTags.Except(new[] { textTag });

                switch (sorting)
                {
                    case Sorting.Asc:
                        // start index within tne target
                        invalidList.AddRange(checkTags.Where(x => x.StartIndex >= textTag.StartIndex && x.StartIndex < textTag.EndIndex));
                        break;

                    case Sorting.Desc:
                        // end index within tne target
                        invalidList.AddRange(checkTags.Where(x => x.EndIndex > textTag.StartIndex && x.EndIndex <= textTag.EndIndex));
                        break;
                }
            }

            return Sort(invalidList.Distinct().ToArray());
        }

        public enum Sorting
        {
            /// <summary>
            /// Mark next time tag is error if conflict.
            /// </summary>
            Asc,

            /// <summary>
            /// Mark previous tag is error if conflict.
            /// </summary>
            Desc
        }
    }
}