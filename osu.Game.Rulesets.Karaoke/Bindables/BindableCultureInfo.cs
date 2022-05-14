﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Bindables
{
    public class BindableCultureInfo : Bindable<CultureInfo>
    {
        public BindableCultureInfo(CultureInfo value = default)
            : base(value)
        {
        }

        public override void Parse(object input)
        {
            if (input == null)
            {
                Value = null;
                return;
            }

            switch (input)
            {
                case string str:
                    Value = new CultureInfo(str);
                    break;

                case int lcid:
                    Value = new CultureInfo(lcid);
                    break;

                case CultureInfo cultureInfo:
                    Value = cultureInfo;
                    break;

                default:
                    base.Parse(input);
                    break;
            }
        }

        public override string ToString()
        {
            return CultureInfoUtils.GetLanguageDisplayText(Value);
        }

        protected override Bindable<CultureInfo> CreateInstance()
        {
            return new BindableCultureInfo();
        }
    }
}
