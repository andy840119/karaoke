// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;

namespace osu.Game.Rulesets.Karaoke.Localisation
{
    public static class CommonStrings
    {
        /// <summary>
        ///     "karaoke!"
        /// </summary>
        public static LocalisableString RulesetName => new TranslatableString(getKey(@"karaoke"), @"karaoke!");

        private const string prefix = @"osu.Game.Rulesets.Karaoke.Localisation.Common";

        private static string getKey(string key)
        {
            return $@"{prefix}:{key}";
        }
    }
}
