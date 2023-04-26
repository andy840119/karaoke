// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Sprites;

public partial class DrawableLayoutPreview : CompositeDrawable
{
    private const float scale = 0.4f;

    private readonly Box background;
    private readonly Box previewLyric;
    private readonly OsuSpriteText notSupportText;

    [Resolved]
    private ISkinSource? skinSource { get; set; }

    public DrawableLayoutPreview()
    {
        InternalChildren = new Drawable[]
        {
            background = new Box
            {
                RelativeSizeAxes = Axes.Both,
            },
            previewLyric = new Box
            {
                Height = 15,
            },
            notSupportText = new OsuSpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            },
        };

        previewLyric.Hide();
        notSupportText.Hide();
    }

    private Lyric? lyric;

    public Lyric? Lyric
    {
        get => lyric;
        set
        {
            if (lyric == value)
                return;

            lyric = value;
            // todo: maybe should user the real lyric?

            // or will it able to cast the transformer?
            // lyric.EffectApplier.UpdateInitialTransforms(previewLyric);
        }
    }

    [BackgroundDependencyLoader]
    private void load(OsuColour colours)
    {
        background.Colour = colours.Gray2;
        previewLyric.Colour = colours.Yellow;
    }
}
