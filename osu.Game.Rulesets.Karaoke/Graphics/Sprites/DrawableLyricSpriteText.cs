// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;

namespace osu.Game.Rulesets.Karaoke.Graphics.Sprites;

public partial class DrawableLyricSpriteText : LyricSpriteText
{
    private readonly IBindable<string> textBindable = new Bindable<string>();
    private readonly IBindable<int> rubyTagsVersion = new Bindable<int>();
    private readonly IBindableList<RubyTag> rubyTagsBindable = new BindableList<RubyTag>();
    private readonly IBindable<int> romajiTagsVersion = new Bindable<int>();
    private readonly IBindableList<TimeTag> timeTagsBindable = new BindableList<TimeTag>();

    public DrawableLyricSpriteText(Lyric lyric)
    {
        textBindable.BindValueChanged(text => { Text = text.NewValue; }, true);
        rubyTagsVersion.BindValueChanged(_ => updateRubies());
        rubyTagsBindable.BindCollectionChanged((_, _) => updateRubies());
        romajiTagsVersion.BindValueChanged(_ => updateRubies());
        timeTagsBindable.BindCollectionChanged((_, _) => updateRomajies());

        textBindable.BindTo(lyric.TextBindable);
        rubyTagsVersion.BindTo(lyric.RubyTagsVersion);
        rubyTagsBindable.BindTo(lyric.RubyTagsBindable);
        romajiTagsVersion.BindTo(lyric.RomajiTagsVersion);
        timeTagsBindable.BindTo(lyric.TimeTagsBindable);
    }

    private void updateRubies()
    {
        Rubies = rubyTagsBindable.Select(TextTagUtils.ToPositionText).ToArray();
    }

    private void updateRomajies()
    {
        Romajies = TimeTagsUtils.ToPositionTexts(timeTagsBindable).ToArray();
    }
}
