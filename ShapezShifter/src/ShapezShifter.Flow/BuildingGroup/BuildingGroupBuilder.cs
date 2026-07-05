using System;
using System.Collections.Generic;
using Core.Localization;
using Game.Core.Content.Buildings;
using Game.Core.Research;
using UnityEngine;

namespace ShapezShifter.Flow
{
    internal class BuildingGroupBuilder
        : IIdentifiableBuildingGroupBuilder,
          IIdentifiableAndTitledBuildingGroupBuilder,
          IIdentifiableTitledAndDescribedBuildingGroupBuilder,
          IIdentifiableAndPresentableBuildingGroupBuilder,
          IIdentifiablePresentableAndCategorizedBuildingGroupBuilder,
          IBuildingGroupBuilder
    {
        internal BuildingGroupBuilder(BuildingDefinitionGroupId groupId)
        {
            GroupId = groupId;
        }

        public BuildingDefinitionGroupId GroupId { get; }
        private Sprite Icon;
        private IText Title;
        private IText Description;
        private bool IsTransportBuilding;
        private DefaultPreferredPlacementMode DefaultPreferredPlacement;
        private bool IsRemovable = true;
        private bool IsSelectable = true;
        private bool IsBuildable = true;
        private bool AllowPlaceOnNonFilledTiles;
        private BuildingDefinitionGroupId PipetteOverrideId;
        private bool AllowPlaceOnNotch;
        private int AutoAttractIOScoreMultiplier;
        private bool AutoConnect = true;
        private bool AutoRotateToFitStructures = true;
        private bool AllowNonForcingReplacementByOtherBuildings;
        private bool ShouldSkipReplacementIOChecks;
        private bool AlwaysProducesConflictIndicators;
        private bool RenderConflictIndicatorMeshes;
        private bool RenderConflictIndicatorVisualization;
        private bool RenderConnectorIndicators;
        private bool RenderConflictingConnectorIndicators;
        private bool ShowNotchIndicators;
        private bool ShowStatBeltProcessingTime;
        private bool ShowStatBuildingsPerFullBelt;
        private bool ShowInSpeedOverview;
        private bool ShowAsResearchReward;
        private UnlockableStoreContentId RequireStoreContentId;
        private WikiEntryId LinkedEntryId;
        private readonly List<Type> PlacementIndicators = new();
        private IEnumerable<IBuildingPlacementRequirement> PlacementRequirements;
        private MetaStructureOverview ThroughputDisplayHelper;

        public IIdentifiableAndPresentableBuildingGroupBuilder WithPresentation(
            IText title,
            IText description,
            Sprite icon)
        {
            this.WithTitle(title)
                .WithDescription(description)
                .WithIcon(icon);
            return this;
        }

        public IIdentifiableAndTitledBuildingGroupBuilder WithTitle(IText title)
        {
            Title = title;
            return this;
        }

        public IIdentifiableTitledAndDescribedBuildingGroupBuilder WithDescription(IText description)
        {
            Description = description;
            return this;
        }

        public IIdentifiableAndPresentableBuildingGroupBuilder WithIcon(Sprite icon)
        {
            Icon = icon;
            return this;
        }

        public IIdentifiablePresentableAndCategorizedBuildingGroupBuilder AsTransportableBuilding()
        {
            IsTransportBuilding = true;
            return this;
        }

        public IIdentifiablePresentableAndCategorizedBuildingGroupBuilder AsNonTransportableBuilding()
        {
            IsTransportBuilding = false;
            return this;
        }

        public IBuildingGroupBuilder WithPreferredPlacement(DefaultPreferredPlacementMode defaultPreferredPlacementMode)
        {
            DefaultPreferredPlacement = defaultPreferredPlacementMode;
            return this;
        }

        public IBuildingGroupBuilder Removable()
        {
            IsRemovable = true;
            return this;
        }

        public IBuildingGroupBuilder NotRemovable()
        {
            IsRemovable = false;
            return this;
        }

        public IBuildingGroupBuilder Selectable()
        {
            IsSelectable = true;
            return this;
        }

        public IBuildingGroupBuilder NotSelectable()
        {
            IsSelectable = false;
            return this;
        }

        public IBuildingGroupBuilder Buildable()
        {
            IsBuildable = true;
            return this;
        }

        public IBuildingGroupBuilder NotBuildable()
        {
            IsBuildable = false;
            return this;
        }

        public IBuildingGroupBuilder AllowedOnNonFilledTiles()
        {
            AllowPlaceOnNonFilledTiles = true;
            return this;
        }

        public IBuildingGroupBuilder NotAllowedOnNonFilledTiles()
        {
            AllowPlaceOnNonFilledTiles = false;
            return this;
        }

        public IBuildingGroupBuilder AllowedOnNotches()
        {
            AllowPlaceOnNotch = true;
            return this;
        }

        public IBuildingGroupBuilder NotAllowedOnNotches()
        {
            AllowPlaceOnNotch = false;
            return this;
        }

        public IBuildingGroupBuilder AutoConnected()
        {
            AutoConnect = true;
            return this;
        }

        public IBuildingGroupBuilder NotAutoConnected()
        {
            AutoConnect = false;
            return this;
        }

        public IBuildingGroupBuilder AutoRotated()
        {
            AutoRotateToFitStructures = true;
            return this;
        }

        public IBuildingGroupBuilder NotAutoRotated()
        {
            AutoRotateToFitStructures = false;
            return this;
        }

        public IBuildingGroupBuilder AllowedToBeReplacedWithoutForce()
        {
            AllowNonForcingReplacementByOtherBuildings = true;
            return this;
        }

        public IBuildingGroupBuilder NotAllowedToBeReplacedWithoutForce()
        {
            AllowNonForcingReplacementByOtherBuildings = false;
            return this;
        }

        public IBuildingGroupBuilder RenderingConflictingIndicatorMeshes()
        {
            RenderConflictIndicatorMeshes = true;
            return this;
        }

        public IBuildingGroupBuilder NotRenderingConflictingIndicatorMeshes()
        {
            RenderConflictIndicatorMeshes = false;
            return this;
        }

        public IBuildingGroupBuilder RenderingConflictingIndicatorVisualization()
        {
            RenderConflictIndicatorVisualization = true;
            return this;
        }

        public IBuildingGroupBuilder NotRenderingConflictingIndicatorVisualization()
        {
            RenderConflictIndicatorVisualization = false;
            return this;
        }

        public IBuildingGroupBuilder ProducingConflictingIndicatorsAlways()
        {
            AlwaysProducesConflictIndicators = true;
            return this;
        }

        public IBuildingGroupBuilder NotProducingConflictingIndicatorsAlways()
        {
            AlwaysProducesConflictIndicators = false;
            return this;
        }

        public IBuildingGroupBuilder RenderingConnectorIndicators()
        {
            RenderConnectorIndicators = true;
            return this;
        }

        public IBuildingGroupBuilder NotRenderingConnectorIndicator()
        {
            RenderConnectorIndicators = false;
            return this;
        }

        public IBuildingGroupBuilder RenderingConnectorConflictIndicators()
        {
            RenderConflictingConnectorIndicators = true;
            return this;
        }

        public IBuildingGroupBuilder NotRenderingConnectorConflictIndicator()
        {
            RenderConflictingConnectorIndicators = false;
            return this;
        }

        public IBuildingGroupBuilder ShowingNotchIndicators()
        {
            ShowNotchIndicators = true;
            return this;
        }

        public IBuildingGroupBuilder NotShowingNotchIndicators()
        {
            ShowNotchIndicators = false;
            return this;
        }

        public IBuildingGroupBuilder ShowingBeltProcessingTimeStat()
        {
            ShowStatBeltProcessingTime = true;
            return this;
        }

        public IBuildingGroupBuilder NotShowingBeltProcessingTimeStat()
        {
            ShowStatBeltProcessingTime = false;
            return this;
        }

        public IBuildingGroupBuilder ShowingBuildingsPerFullBeltStat()
        {
            ShowStatBuildingsPerFullBelt = true;
            return this;
        }

        public IBuildingGroupBuilder NotShowingBuildingsPerFullBeltStat()
        {
            ShowStatBuildingsPerFullBelt = false;
            return this;
        }

        public IBuildingGroupBuilder ShowingInSpeedOverview()
        {
            ShowInSpeedOverview = true;
            return this;
        }

        public IBuildingGroupBuilder NotShowingInSpeedOverview()
        {
            ShowInSpeedOverview = false;
            return this;
        }

        public IBuildingGroupBuilder DisplayableAsReward()
        {
            ShowAsResearchReward = true;
            return this;
        }

        public IBuildingGroupBuilder NotDisplayableAsReward()
        {
            ShowAsResearchReward = false;
            return this;
        }

        public IBuildingGroupBuilder SkippingReplacementConnectorChecks()
        {
            ShouldSkipReplacementIOChecks = true;
            return this;
        }

        public IBuildingGroupBuilder NotSkippingReplacementConnectorChecks()
        {
            ShouldSkipReplacementIOChecks = false;
            return this;
        }

        public IBuildingGroupBuilder WithConnectionMultiplier(int autoAttractScore)
        {
            AutoAttractIOScoreMultiplier = autoAttractScore;
            return this;
        }

        public IBuildingGroupBuilder WithPipetteOverride(BuildingDefinitionGroupId overrideGroup)
        {
            PipetteOverrideId = overrideGroup;
            return this;
        }

        public IBuildingGroupBuilder WithPlacementIndicator<TPlacementIndicator>()
            where TPlacementIndicator : IBuildingPlacementIndicator
        {
            PlacementIndicators.Add(typeof(TPlacementIndicator));
            return this;
        }

        public IBuildingGroupBuilder WithPlacementRequirements(IEnumerable<IBuildingPlacementRequirement> requirements)
        {
            PlacementRequirements = requirements;
            return this;
        }

        public IBuildingGroupBuilder WithCustomStructureOverview(MetaStructureOverview structureOverview)
        {
            ThroughputDisplayHelper = structureOverview;
            return this;
        }

        public IBuildingGroupBuilder WithDefaultStructureOverview()
        {
            return WithCustomStructureOverview(
                new MetaStructureOverview { Slots = Array.Empty<MetaStructureOverview.IOData>() });
        }

        public BuildingDefinitionGroup BuildAndRegister(GameBuildings gameBuildings)
        {
            Debugging.Logger.Info?.Log($"Registering {GroupId} to buildings");
            BuildingDefinitionGroup buildingGroup = new(
                id: GroupId,
                icon: Icon,
                title: Title,
                description: Description,
                isTransportBuilding: IsTransportBuilding,
                selectable: IsSelectable,
                playerBuildable: IsBuildable,
                removable: IsRemovable,
                allowPlaceOnNonFilledTiles: AllowPlaceOnNonFilledTiles,
                pipetteOverrideId: PipetteOverrideId,
                defaultPreferredPlacementMode: DefaultPreferredPlacement,
                allowPlaceOnNotch: AllowPlaceOnNotch,
                autoAttractIOScoreMultiplier: AutoAttractIOScoreMultiplier,
                autoConnect: AutoConnect,
                autoRotateToFitStructures: AutoRotateToFitStructures,
                allowNonForcingReplacementByOtherBuildings: AllowNonForcingReplacementByOtherBuildings,
                shouldSkipReplacementIOChecks: ShouldSkipReplacementIOChecks,
                alwaysProducesConflictIndicators: AlwaysProducesConflictIndicators,
                renderConflictIndicatorMeshes: RenderConflictIndicatorMeshes,
                renderConflictIndicatorVisualization: RenderConflictIndicatorVisualization,
                renderConnectorIndicators: RenderConnectorIndicators,
                renderConflictingConnectorIndicators: RenderConflictingConnectorIndicators,
                showNotchIndicators: ShowNotchIndicators,
                showStatBeltProcessingTime: ShowStatBeltProcessingTime,
                showStatBuildingsPerFullBelt: ShowStatBuildingsPerFullBelt,
                showInSpeedOverview: ShowInSpeedOverview,
                showAsResearchReward: ShowAsResearchReward,
                linkedWikiEntry: LinkedEntryId,
                placementIndicatorTypes: PlacementIndicators.ToArray() ?? Array.Empty<Type>(),
                placementRequirements: PlacementRequirements ?? Array.Empty<IBuildingPlacementRequirement>(),
                structureOverview: ThroughputDisplayHelper);

            gameBuildings._All.Add(buildingGroup);
            gameBuildings._VariantsById.Add(key: buildingGroup.Id, value: buildingGroup);
            return buildingGroup;
        }
    }
}
