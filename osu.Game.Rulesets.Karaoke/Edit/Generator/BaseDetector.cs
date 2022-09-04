// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator
{
    public abstract class BaseDetector<TProperty> : ILyricPropertyDetector<TProperty>
    {
        public LocalisableString? GetInvalidMessage(Lyric lyric)
        {
            var locked = HitObjectWritableUtils.GetLyricPropertyLockedReason(lyric, DetectPropertyName);
            return locked switch
            {
                LockLyricPropertyBy.ReferenceLyricConfig => "Cannot detect property because the property is sync from other lyric.",
                LockLyricPropertyBy.LockState => "Cannot detect property because the property is locked.",
                _ => GetExtraCheck(lyric)
            };
        }

        public TProperty Detect(Lyric lyric)
        {
            if (GetInvalidMessage(lyric) != null)
                throw new NotSupportedException();

            return PerformDetect(lyric);
        }

        protected abstract string[] DetectPropertyName { get; }

        protected virtual LocalisableString? GetExtraCheck(Lyric lyric)
        {
            return null;
        }

        protected abstract TProperty PerformDetect(Lyric lyric);
    }
}
