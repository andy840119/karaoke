﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class RubyTag : ITextTag
    {
        [JsonIgnore]
        public readonly Bindable<string> TextBindable = new Bindable<string>();

        /// <summary>
        /// If kanji Matched, then apply ruby
        /// </summary>
        public string Text
        {
            get => TextBindable.Value;
            set => TextBindable.Value = value;
        }

        [JsonIgnore]
        public readonly BindableInt StartIndexBindable = new BindableInt();

        /// <summary>
        /// Start index
        /// </summary>
        public int StartIndex
        {
            get => StartIndexBindable.Value;
            set => StartIndexBindable.Value = value;
        }

        [JsonIgnore]
        public readonly BindableInt EndIndexBindable = new BindableInt();

        /// <summary>
        /// End index
        /// </summary>
        public int EndIndex
        {
            get => EndIndexBindable.Value;
            set => EndIndexBindable.Value = value;
        }
    }
}
