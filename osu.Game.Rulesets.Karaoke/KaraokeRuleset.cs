﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Bindings;
using osu.Framework.IO.Stores;
using osu.Framework.Testing;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Configuration;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Difficulty;
using osu.Game.Rulesets.Karaoke.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Setup;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Karaoke.Skinning.Legacy;
using osu.Game.Rulesets.Karaoke.Statistics;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Replays.Types;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using osu.Game.Scoring;
using osu.Game.Screens.Edit.Setup;
using osu.Game.Screens.Ranking.Statistics;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke
{
    [ExcludeFromDynamicCompile]
    public class KaraokeRuleset : Ruleset
    {
        public const string SHORT_NAME = "karaoke";

        public const int GAMEPLAY_INPUT_VARIANT = 1;

        public const int EDIT_INPUT_VARIANT = 2;

        public override IEnumerable<int> AvailableVariants => new[] { GAMEPLAY_INPUT_VARIANT, EDIT_INPUT_VARIANT };

        public override string Description => "karaoke!";

        public override string ShortName => "karaoke!";

        public override string PlayingVerb => "Singing karaoke";

        public KaraokeRuleset()
        {
            // It's a tricky way to let lazer to read karaoke testing beatmap
            KaraokeLegacyBeatmapDecoder.Register();
            KaraokeJsonBeatmapDecoder.Register();

            // it's a tricky way for loading customized karaoke beatmap.
            RulesetInfo.OnlineID = 111;
        }

        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
        {
            return new DrawableKaraokeRuleset(this, beatmap, mods);
        }

        public override ScoreProcessor CreateScoreProcessor()
        {
            return new KaraokeScoreProcessor();
        }

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap)
        {
            return new KaraokeBeatmapConverter(beatmap, this);
        }

        public override IBeatmapProcessor CreateBeatmapProcessor(IBeatmap beatmap)
        {
            return new KaraokeBeatmapProcessor(beatmap);
        }

        public override PerformanceCalculator CreatePerformanceCalculator()
        {
            return new KaraokePerformanceCalculator();
        }

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0)
        {
            return variant switch
            {
                0 =>
                    // Vocal
                    Array.Empty<KeyBinding>(),
                GAMEPLAY_INPUT_VARIANT => new[]
                {
                    // Basic control
                    new KeyBinding(InputKey.Number1, KaraokeAction.FirstLyric),
                    new KeyBinding(InputKey.Left, KaraokeAction.PreviousLyric),
                    new KeyBinding(InputKey.Right, KaraokeAction.NextLyric),
                    new KeyBinding(InputKey.Space, KaraokeAction.PlayAndPause),

                    // Panel
                    new KeyBinding(InputKey.P, KaraokeAction.OpenPanel),

                    // Advance control
                    new KeyBinding(InputKey.Q, KaraokeAction.IncreaseTempo),
                    new KeyBinding(InputKey.A, KaraokeAction.DecreaseTempo),
                    new KeyBinding(InputKey.Z, KaraokeAction.ResetTempo),
                    new KeyBinding(InputKey.W, KaraokeAction.IncreasePitch),
                    new KeyBinding(InputKey.S, KaraokeAction.DecreasePitch),
                    new KeyBinding(InputKey.X, KaraokeAction.ResetPitch),
                    new KeyBinding(InputKey.E, KaraokeAction.IncreaseVocalPitch),
                    new KeyBinding(InputKey.D, KaraokeAction.DecreaseVocalPitch),
                    new KeyBinding(InputKey.C, KaraokeAction.ResetVocalPitch),
                    new KeyBinding(InputKey.R, KaraokeAction.IncreaseSaitenPitch),
                    new KeyBinding(InputKey.F, KaraokeAction.DecreaseSaitenPitch),
                    new KeyBinding(InputKey.V, KaraokeAction.ResetSaitenPitch)
                },
                EDIT_INPUT_VARIANT => new[]
                {
                    // moving
                    new KeyBinding(InputKey.Up, KaraokeEditAction.Up),
                    new KeyBinding(InputKey.Down, KaraokeEditAction.Down),
                    new KeyBinding(InputKey.Left, KaraokeEditAction.Left),
                    new KeyBinding(InputKey.Right, KaraokeEditAction.Right),
                    new KeyBinding(InputKey.PageUp, KaraokeEditAction.First),
                    new KeyBinding(InputKey.PageDown, KaraokeEditAction.Last),

                    new KeyBinding(new[] { InputKey.Alt, InputKey.BracketLeft }, KaraokeEditAction.PreviousEditMode),
                    new KeyBinding(new[] { InputKey.Alt, InputKey.BracketRight }, KaraokeEditAction.NextEditMode),

                    // Edit Ruby / romaji tag.
                    new KeyBinding(new[] { InputKey.Z, InputKey.Left }, KaraokeEditAction.EditTextTagReduceStartIndex),
                    new KeyBinding(new[] { InputKey.Z, InputKey.Right }, KaraokeEditAction.EditTextTagIncreaseStartIndex),
                    new KeyBinding(new[] { InputKey.X, InputKey.Left }, KaraokeEditAction.EditTextTagReduceEndIndex),
                    new KeyBinding(new[] { InputKey.X, InputKey.Right }, KaraokeEditAction.EditTextTagIncreaseEndIndex),

                    // edit time-tag.
                    new KeyBinding(InputKey.N, KaraokeEditAction.Create),
                    new KeyBinding(InputKey.Delete, KaraokeEditAction.Remove),
                    new KeyBinding(new[] { InputKey.Z }, KaraokeEditAction.ShiftTheTimeTagLeft),
                    new KeyBinding(new[] { InputKey.X }, KaraokeEditAction.ShiftTheTimeTagRight),
                    new KeyBinding(new[] { InputKey.A }, KaraokeEditAction.ShiftTheTimeTagStateLeft),
                    new KeyBinding(new[] { InputKey.S }, KaraokeEditAction.ShiftTheTimeTagStateRight),
                    new KeyBinding(InputKey.Enter, KaraokeEditAction.SetTime),
                    new KeyBinding(InputKey.BackSpace, KaraokeEditAction.ClearTime)
                },
                _ => Array.Empty<KeyBinding>()
            };
        }

        public override string GetVariantName(int variant)
        {
            return variant switch
            {
                GAMEPLAY_INPUT_VARIANT => "Gameplay",
                EDIT_INPUT_VARIANT => "Composer",
                _ => throw new ArgumentNullException(nameof(variant))
            };
        }

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            return type switch
            {
                ModType.DifficultyReduction => new Mod[]
                {
                    new KaraokeModNoFail()
                },
                ModType.DifficultyIncrease => new Mod[]
                {
                    new KaraokeModHiddenNote(),
                    new KaraokeModFlashlight(),
                    new MultiMod(new KaraokeModSuddenDeath(), new KaraokeModPerfect(), new KaraokeModWindowsUpdate())
                },
                ModType.Automation => new Mod[]
                {
                    new MultiMod(new KaraokeModAutoplay(), new KaraokeModAutoplayBySinger())
                },
                ModType.Fun => new Mod[]
                {
                    new KaraokeModPractice(),
                    new KaraokeModDisableNote(),
                    new KaraokeModSnow()
                },
                _ => Array.Empty<Mod>()
            };
        }

        public override Drawable CreateIcon()
        {
            return new Container
            {
                AutoSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Scale = new Vector2(0.9f),
                        Texture = new TextureStore(new TextureLoaderStore(CreateResourceStore()), false).Get("Textures/logo")
                    },
                    new SpriteIcon
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Scale = new Vector2(45f),
                        Icon = FontAwesome.Regular.Circle
                    }
                }
            };
        }

        public override IResourceStore<byte[]> CreateResourceStore()
        {
            var store = new ResourceStore<byte[]>();

            // add ruleset store
            store.AddStore(getRulesetStore());

            // add shader resource from font package.
            store.AddStore(new NamespacedResourceStore<byte[]>(new ShaderResourceStore(), "Resources"));

            return store;

            IResourceStore<byte[]> getRulesetStore()
            {
                var rulesetStore = base.CreateResourceStore();
                if (rulesetStore.GetAvailableResources().Any())
                    return rulesetStore;

                // IRMerge might change the assembly name, which will cause resource not found.
                return new NamespacedResourceStore<byte[]>(new DllResourceStore("osu.Game.Rulesets.Karaoke.dll"), @"Resources");
            }
        }

        public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap)
        {
            return new KaraokeDifficultyCalculator(RulesetInfo, beatmap);
        }

        public override HitObjectComposer CreateHitObjectComposer()
        {
            return new KaraokeHitObjectComposer(this);
        }

        public override IBeatmapVerifier CreateBeatmapVerifier()
        {
            return new KaraokeBeatmapVerifier();
        }

        public override ISkin CreateLegacySkinProvider(ISkin skin, IBeatmap beatmap)
        {
            return new KaraokeLegacySkinTransformer(skin, beatmap);
        }

        public override IConvertibleReplayFrame CreateConvertibleReplayFrame()
        {
            return new KaraokeReplayFrame();
        }

        public override IRulesetConfigManager CreateConfig(SettingsStore settings)
        {
            return new KaraokeRulesetConfigManager(settings, RulesetInfo);
        }

        public override RulesetSettingsSubsection CreateSettings()
        {
            return new KaraokeSettingsSubsection(this);
        }

        public override string GetDisplayNameForHitResult(HitResult result)
        {
            return result switch
            {
                HitResult.Great => "Great",
                HitResult.Ok => "OK",
                HitResult.Meh => "Meh",
                _ => base.GetDisplayNameForHitResult(result)
            };
        }

        public override StatisticRow[] CreateStatisticsForScore(ScoreInfo score, IBeatmap playableBeatmap)
        {
            const int fix_height = 560;
            const int text_size = 14;
            const int spacing = 15;
            const int info_height = 200;

            // Always display song info
            var statistic = new List<StatisticRow>
            {
                new()
                {
                    Columns = new[]
                    {
                        new StatisticItem("Info", () => new BeatmapInfoGraph(playableBeatmap)
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = info_height
                        }, dimension: new Dimension(GridSizeMode.Relative, 0.6f)),
                        new StatisticItem("", () => new Container(), dimension: new Dimension(GridSizeMode.Absolute, 10)),
                        new StatisticItem("Metadata", () => new BeatmapMetadataGraph(playableBeatmap)
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = info_height
                        }, dimension: new Dimension())
                    }
                }
            };

            // Set component to remain height
            const int remain_height = fix_height - text_size - spacing - info_height;

            if (playableBeatmap.IsScorable())
            {
                statistic.Add(new StatisticRow
                {
                    Columns = new[]
                    {
                        new StatisticItem("Saiten Result", () => new SaitenResultGraph(score, playableBeatmap)
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = remain_height - text_size - spacing
                        })
                    }
                });
            }
            else
            {
                statistic.Add(new StatisticRow
                {
                    Columns = new[]
                    {
                        new StatisticItem("Result", () => new NotScorableGraph
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = remain_height - text_size - spacing
                        })
                    }
                });
            }

            return statistic.ToArray();
        }

        public override RulesetSetupSection CreateEditorSetupSection()
        {
            return new KaraokeSetupSection();
        }

        protected override IEnumerable<HitResult> GetValidHitResults()
        {
            return new[]
            {
                HitResult.Great,
                HitResult.Ok,
                HitResult.Meh
            };
        }
    }
}
