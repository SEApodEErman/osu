﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Catch.Objects;
using osu.Game.Rulesets.Catch.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Scoring;

namespace osu.Game.Rulesets.Catch.UI
{
    public class DrawableCatchRuleset : DrawableScrollingRuleset<CatchHitObject>
    {
        protected override ScrollVisualisationMethod VisualisationMethod => ScrollVisualisationMethod.Constant;

        protected override bool UserScrollSpeedAdjustment => false;

        private bool showMobileMapper = true;

        public DrawableCatchRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            : base(ruleset, beatmap, mods)
        {
            // Check if mods have RelaxMod instance
            if (mods != null && mods.OfType<ModRelax>().Any())
                showMobileMapper = false;

            Direction.Value = ScrollingDirection.Down;
            TimeRange.Value = IBeatmapDifficultyInfo.DifficultyRange(beatmap.Difficulty.ApproachRate, 1800, 1200, 450);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            if (showMobileMapper)
                KeyBindingInputManager.Add(new CatchTouchInputMapper());
        }

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new CatchFramedReplayInputHandler(replay);

        protected override ReplayRecorder CreateReplayRecorder(Score score) => new CatchReplayRecorder(score, (CatchPlayfield)Playfield);

        protected override Playfield CreatePlayfield() => new CatchPlayfield(Beatmap.Difficulty);

        public override PlayfieldAdjustmentContainer CreatePlayfieldAdjustmentContainer() => new CatchPlayfieldAdjustmentContainer();

        protected override PassThroughInputManager CreateInputManager() => new CatchInputManager(Ruleset.RulesetInfo);

        public override DrawableHitObject<CatchHitObject> CreateDrawableRepresentation(CatchHitObject h) => null;
    }
}
