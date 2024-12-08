// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Graphics.Sprites;

public class LyricFontInfo
{
    public static LyricFontInfo CreateDefault() => new()
    {
        SmartHorizon = KaraokeTextSmartHorizon.Multi,
        LyricsInterval = 4,
        RubyInterval = 2,
        RomanisationInterval = 2,
        RubyAlignment = LyricTextAlignment.EqualSpace,
        RomanisationAlignment = LyricTextAlignment.EqualSpace,
        RubyMargin = 4,
        RomanisationMargin = 4,
        MainTextFont = new FontUsage("Torus", 48, "Bold"),
        RubyTextFont = new FontUsage("Torus", 20, "Bold"),
        RomanisationTextFont = new FontUsage("Torus", 20, "Bold"),
    };

    /// <summary>
    /// ???
    /// </summary>
    public KaraokeTextSmartHorizon SmartHorizon { get; set; } = KaraokeTextSmartHorizon.None;

    /// <summary>
    /// Interval between lyric texts
    /// </summary>
    public int LyricsInterval { get; set; }

    /// <summary>
    /// Interval between lyric rubies
    /// </summary>
    public int RubyInterval { get; set; }

    /// <summary>
    /// Interval between lyric romanisation
    /// </summary>
    public int RomanisationInterval { get; set; }

    /// <summary>
    /// Ruby position alignment
    /// </summary>
    public LyricTextAlignment RubyAlignment { get; set; } = LyricTextAlignment.Auto;

    /// <summary>
    /// Ruby position alignment
    /// </summary>
    public LyricTextAlignment RomanisationAlignment { get; set; } = LyricTextAlignment.Auto;

    /// <summary>
    /// Interval between lyric text and ruby
    /// </summary>
    public int RubyMargin { get; set; }

    /// <summary>
    /// (Additional) Interval between lyric text and romanisation.
    /// </summary>
    public int RomanisationMargin { get; set; }

    /// <summary>
    /// Main text font
    /// </summary>
    public FontUsage MainTextFont { get; set; } = new("Torus", 48, "Bold");

    /// <summary>
    /// Ruby text font
    /// </summary>
    public FontUsage RubyTextFont { get; set; } = new("Torus", 20, "Bold");

    /// <summary>
    /// Romanisation text font
    /// </summary>
    public FontUsage RomanisationTextFont { get; set; } = new("Torus", 20, "Bold");
}
