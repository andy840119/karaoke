// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas
{
    public class Singer : ISinger
    {
        public int ID { get; }

        [JsonIgnore]
        public readonly Bindable<int> OrderBindable = new();

        [JsonIgnore]
        public readonly Bindable<string> AvatarBindable = new();

        [JsonIgnore]
        public readonly Bindable<string> NameBindable = new();

        [JsonIgnore]
        public readonly Bindable<string> RomajiNameBindable = new();

        [JsonIgnore]
        public readonly Bindable<string> EnglishNameBindable = new();

        [JsonIgnore]
        public readonly Bindable<string> DescriptionBindable = new();

        [JsonIgnore]
        public Bindable<float> HueBindable = new BindableFloat
        {
            MinValue = 0,
            MaxValue = 1
        };

        /// <summary>
        ///     Order
        /// </summary>
        public int Order
        {
            get => OrderBindable.Value;
            set => OrderBindable.Value = value;
        }

        public string Avatar
        {
            get => AvatarBindable.Value;
            set => AvatarBindable.Value = value;
        }

        public float Hue
        {
            get => HueBindable.Value;
            set => HueBindable.Value = value;
        }

        public string Name
        {
            get => NameBindable.Value;
            set => NameBindable.Value = value;
        }

        public string RomajiName
        {
            get => RomajiNameBindable.Value;
            set => RomajiNameBindable.Value = value;
        }

        public string EnglishName
        {
            get => EnglishNameBindable.Value;
            set => EnglishNameBindable.Value = value;
        }

        public string Description
        {
            get => DescriptionBindable.Value;
            set => DescriptionBindable.Value = value;
        }

        public Singer()
        {
        }

        public Singer(int id)
        {
            ID = id;
        }
    }
}
