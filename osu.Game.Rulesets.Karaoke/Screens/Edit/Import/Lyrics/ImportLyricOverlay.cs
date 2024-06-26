// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Input.Events;
using osu.Game.Beatmaps;
using osu.Game.Input.Bindings;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics;

[Cached(typeof(IImportStateResolver))]
public partial class ImportLyricOverlay : FullscreenOverlay<ImportLyricHeader>, IImportStateResolver
{
    public Action<IBeatmap>? OnImportFinished;

    public Action? OverlayClosed;

    [Cached]
    protected LyricImporterSubScreenStack ScreenStack { get; private set; }

    private readonly BindableBeatDivisor beatDivisor = new();

    private EditorBeatmap editorBeatmap = null!;

    private ImportLyricManager importManager = null!;

    private LyricsProvider lyricsProvider = null!;

    private DependencyContainer dependencies = null!;

    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

    public bool IsFirstStep() => ScreenStack.IsFirstStep();

    protected override bool DimMainContent => false;

    public ImportLyricOverlay()
        : base(OverlayColourScheme.Pink)
    {
        Width = 1;

        Child = new GridContainer
        {
            RelativeSizeAxes = Axes.Both,
            RowDimensions = new[]
            {
                new Dimension(GridSizeMode.AutoSize),
                new Dimension(),
            },
            Content = new[]
            {
                new Drawable[]
                {
                    Header,
                },
                new Drawable[]
                {
                    new PopoverContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        Child = ScreenStack = new LyricImporterSubScreenStack
                        {
                            RelativeSizeAxes = Axes.Both,
                        },
                    },
                },
            },
        };
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        // todo: remove caching of this and consume via editorBeatmap?
        // follow how editor.cs do.
        dependencies.Cache(beatDivisor);

        // inject local editor beatmap handler because should not affect global beatmap data.
        var playableBeatmap = new KaraokeBeatmap
        {
            BeatmapInfo =
            {
                Ruleset = new KaraokeRuleset().RulesetInfo,
            },
        };
        AddInternal(editorBeatmap = new EditorBeatmap(playableBeatmap));
        dependencies.CacheAs(editorBeatmap);

        AddInternal(importManager = new ImportLyricManager());
        dependencies.Cache(importManager);

        AddInternal(lyricsProvider = new LyricsProvider());
        dependencies.CacheAs<ILyricsProvider>(lyricsProvider);

        dependencies.Cache(new KaraokeRulesetEditGeneratorConfigManager());

        // Load the screen until everything ready.
        ScreenStack.Push(LyricImporterStep.ImportLyric);
    }

    protected override ImportLyricHeader CreateHeader() => new();

    public override bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
    {
        if (e.Repeat)
            return false;

        switch (e.Action)
        {
            case GlobalAction.Back:
                if (!IsFirstStep())
                {
                    // go to previous step.
                    ScreenStack.Pop();
                }
                else
                {
                    Cancel();
                }

                return true;

            default:
                return base.OnPressed(e);
        }
    }

    public void Cancel()
    {
        Hide();
    }

    public void Finish()
    {
        OnImportFinished?.Invoke(editorBeatmap);
        Hide();
    }

    protected override void PopOutComplete()
    {
        if (LoadState < LoadState.Ready)
            return;

        OverlayClosed?.Invoke();
    }
}
