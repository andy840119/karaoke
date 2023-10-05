// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;

namespace osu.Game.Rulesets.Karaoke.Graphics.Sprites;

public partial class DrawableKaraokeSpriteText<TSpriteText> : KaraokeSpriteText<TSpriteText> where TSpriteText : LyricSpriteText, new()
{
    private const int whole_chunk_index = -1;

    private readonly IBindable<string> textBindable = new Bindable<string>();
    private readonly IBindable<int> timeTagsTimingVersion = new Bindable<int>();
    private readonly IBindableList<TimeTag> timeTagsBindable = new BindableList<TimeTag>();
    private readonly IBindable<int> rubyTagsVersion = new Bindable<int>();
    private readonly IBindableList<RubyTag> rubyTagsBindable = new BindableList<RubyTag>();
    private readonly IBindable<int> romajiTagsVersion = new Bindable<int>();

    private readonly int chunkIndex;

    protected DrawableKaraokeSpriteText(Lyric lyric, int chunkIndex = whole_chunk_index)
    {
        this.chunkIndex = chunkIndex;

        textBindable.BindValueChanged(_ => UpdateText(), true);
        timeTagsTimingVersion.BindValueChanged(_ => UpdateTimeTags());
        timeTagsBindable.BindCollectionChanged((_, _) =>
        {
            UpdateTimeTags();
            UpdateRomajies();
        });
        rubyTagsVersion.BindValueChanged(_ => UpdateRubies());
        rubyTagsBindable.BindCollectionChanged((_, _) => UpdateRubies());
        romajiTagsVersion.BindValueChanged(_ => UpdateRomajies());

        textBindable.BindTo(lyric.TextBindable);
        timeTagsTimingVersion.BindTo(lyric.TimeTagsTimingVersion);
        timeTagsBindable.BindTo(lyric.TimeTagsBindable);
        rubyTagsVersion.BindTo(lyric.RubyTagsVersion);
        rubyTagsBindable.BindTo(lyric.RubyTagsBindable);
        romajiTagsVersion.BindTo(lyric.RomajiTagsVersion);
    }

    protected virtual void UpdateText()
    {
        if (chunkIndex == whole_chunk_index)
        {
            Text = textBindable.Value;
        }
        else
        {
            throw new NotImplementedException("Chunk lyric will be available until V2");
        }
    }

    protected virtual void UpdateTimeTags()
    {
        if (chunkIndex == whole_chunk_index)
        {
            TimeTags = TimeTagsUtils.ToTimeBasedDictionary(timeTagsBindable.ToList());
        }
        else
        {
            throw new NotImplementedException("Chunk lyric will be available until V2");
        }
    }

    protected virtual void UpdateRubies()
    {
        if (chunkIndex == whole_chunk_index)
        {
            Rubies = DisplayRuby ? rubyTagsBindable.Select(TextTagUtils.ToPositionText).ToArray() : Array.Empty<PositionText>();
        }
        else
        {
            throw new NotImplementedException("Chunk lyric will be available until V2");
        }
    }

    protected virtual void UpdateRomajies()
    {
        if (chunkIndex == whole_chunk_index)
        {
            Romajies = DisplayRomaji ? TimeTagsUtils.ToPositionTexts(timeTagsBindable).ToArray() : Array.Empty<PositionText>();
        }
        else
        {
            throw new NotImplementedException("Chunk lyric will be available until V2");
        }
    }

    private bool displayRuby = true;

    public bool DisplayRuby
    {
        get => displayRuby;
        set
        {
            if (displayRuby == value)
                return;

            displayRuby = value;
            Schedule(UpdateRubies);
        }
    }

    private bool displayRomaji = true;

    public bool DisplayRomaji
    {
        get => displayRomaji;
        set
        {
            if (displayRomaji == value)
                return;

            displayRomaji = value;
            Schedule(UpdateRomajies);
        }
    }
}
