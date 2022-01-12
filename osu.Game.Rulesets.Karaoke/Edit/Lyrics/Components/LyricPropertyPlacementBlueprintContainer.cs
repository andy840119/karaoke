// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input;
using osu.Game.Rulesets.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components
{
    public abstract class LyricPropertyPlacementBlueprintContainer<T> : LyricPropertyBlueprintContainer<T> where T : class
    {
        private readonly Container<PlacementBlueprint> placementBlueprintContainer;

        private PlacementBlueprint currentPlacement;
        private InputManager inputManager;

        protected LyricPropertyPlacementBlueprintContainer()
        {
            placementBlueprintContainer = new Container<PlacementBlueprint>
            {
                RelativeSizeAxes = Axes.Both
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(placementBlueprintContainer);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            inputManager = GetContainingInputManager();
        }

        protected bool CursorInPlacementArea => ReceivePositionalInputAt(inputManager.CurrentState.Mouse.Position);

        protected override void Update()
        {
            base.Update();

            if (currentPlacement != null)
            {
                switch (currentPlacement.PlacementActive)
                {
                    case PlacementBlueprint.PlacementState.Waiting:
                        if (!CursorInPlacementArea)
                            removePlacement();
                        break;

                    case PlacementBlueprint.PlacementState.Finished:
                        removePlacement();
                        break;
                }
            }

            // todo: should start placement if
            if (CursorInPlacementArea)
                ensurePlacementCreated();

            if (currentPlacement != null)
                updatePlacementPosition();
        }

        private void ensurePlacementCreated()
        {
            if (currentPlacement != null) return;

            var blueprint = CreatePlacementBlueprint();

            if (blueprint == null)
                return;

            placementBlueprintContainer.Child = currentPlacement = blueprint;

            // Fixes a 1-frame position discrepancy due to the first mouse move event happening in the next frame
            updatePlacementPosition();
        }

        private void updatePlacementPosition()
        {
            var position = SnapScreenSpacePositionToValidTime(inputManager.CurrentState.Mouse.Position);
            var snapResult = new SnapResult(position, null);
            currentPlacement.UpdateTimeAndPosition(snapResult);
        }

        protected abstract Vector2 SnapScreenSpacePositionToValidTime(Vector2 screenSpacePosition);

        private void removePlacement()
        {
            if (currentPlacement == null) return;

            currentPlacement.EndPlacement(false);
            currentPlacement.Expire();
            currentPlacement = null;
        }

        public abstract PlacementBlueprint CreatePlacementBlueprint();
    }
}
