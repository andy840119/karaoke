// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.Skinning.Fonts;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Scoring;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class DrawableKaraokeRuleset : DrawableScrollingRuleset<KaraokeHitObject>
    {
        public new KaraokePlayfield Playfield => (KaraokePlayfield)base.Playfield;

        public new KaraokeRulesetConfigManager Config => (KaraokeRulesetConfigManager)base.Config;
        public KaraokeSessionStatics Session { get; private set; }

        protected virtual bool DisplayNotePlayfield => Beatmap.IsScorable();

        private readonly Bindable<KaraokeScrollingDirection> configDirection = new();

        [Cached(typeof(INotePositionInfo))]
        private readonly NotePositionInfo positionCalculator;

        [Cached]
        private readonly FontManager fontManager;

        public DrawableKaraokeRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods)
            : base(ruleset, beatmap, mods)
        {
            AddInternal(positionCalculator = new NotePositionInfo());
            AddInternal(fontManager = new FontManager());
        }

        public override DrawableHitObject<KaraokeHitObject> CreateDrawableRepresentation(KaraokeHitObject h)
        {
            return null;
        }

        // todo : for now get the fonts in here, might move to better place.
        public override PlayfieldAdjustmentContainer CreatePlayfieldAdjustmentContainer()
        {
            return new KaraokePlayfieldAdjustmentContainer();
        }

        protected override Playfield CreatePlayfield()
        {
            return new KaraokePlayfield();
        }

        protected override PassThroughInputManager CreateInputManager()
        {
            return new KaraokeInputManager(Ruleset.RulesetInfo);
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            var dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
            dependencies.Cache(Session = new KaraokeSessionStatics(Config, Beatmap));
            return dependencies;
        }

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay)
        {
            return new KaraokeFramedReplayInputHandler(replay);
        }

        protected override ReplayRecorder CreateReplayRecorder(Score score)
        {
            return new KaraokeReplayRecorder(score);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            // TODO : it should be moved into NotePlayfield
            new BarLineGenerator<BarLine>(Beatmap).BarLines.ForEach(bar => base.Playfield.Add(bar));

            Config.BindWith(KaraokeRulesetSetting.ScrollDirection, configDirection);
            configDirection.BindValueChanged(direction => Direction.Value = (ScrollingDirection)direction.NewValue, true);

            Config.BindWith(KaraokeRulesetSetting.ScrollTime, TimeRange);

            // Hide note playfield.
            if (!DisplayNotePlayfield)
                Playfield.NotePlayfield.Hide();
        }
    }
}
