using Game.Core.Content.Buildings;
using System.Collections.Generic;

namespace ShapezShifter.Flow
{
    public interface IBuildingGroupBuilder : IBuildingGroupBaseDataProvider
    {
        IBuildingGroupBuilder Removable();

        IBuildingGroupBuilder NotRemovable();

        IBuildingGroupBuilder Selectable();

        IBuildingGroupBuilder NotSelectable();

        IBuildingGroupBuilder Buildable();

        IBuildingGroupBuilder NotBuildable();

        IBuildingGroupBuilder AllowedOnNonFilledTiles();

        IBuildingGroupBuilder NotAllowedOnNonFilledTiles();

        IBuildingGroupBuilder AllowedOnNotches();

        IBuildingGroupBuilder NotAllowedOnNotches();

        IBuildingGroupBuilder AutoConnected();

        IBuildingGroupBuilder NotAutoConnected();

        IBuildingGroupBuilder AutoRotated();

        IBuildingGroupBuilder NotAutoRotated();

        IBuildingGroupBuilder AllowedToBeReplacedWithoutForce();

        IBuildingGroupBuilder NotAllowedToBeReplacedWithoutForce();

        IBuildingGroupBuilder RenderingConflictingIndicatorMeshes();

        IBuildingGroupBuilder NotRenderingConflictingIndicatorMeshes();

        IBuildingGroupBuilder RenderingConflictingIndicatorVisualization();

        IBuildingGroupBuilder NotRenderingConflictingIndicatorVisualization();

        IBuildingGroupBuilder ProducingConflictingIndicatorsAlways();

        IBuildingGroupBuilder NotProducingConflictingIndicatorsAlways();

        IBuildingGroupBuilder RenderingConnectorIndicators();

        IBuildingGroupBuilder NotRenderingConnectorIndicator();

        IBuildingGroupBuilder RenderingConnectorConflictIndicators();

        IBuildingGroupBuilder NotRenderingConnectorConflictIndicator();

        IBuildingGroupBuilder ShowingNotchIndicators();

        IBuildingGroupBuilder NotShowingNotchIndicators();

        IBuildingGroupBuilder ShowingBeltProcessingTimeStat();

        IBuildingGroupBuilder NotShowingBeltProcessingTimeStat();

        IBuildingGroupBuilder ShowingBuildingsPerFullBeltStat();

        IBuildingGroupBuilder NotShowingBuildingsPerFullBeltStat();

        IBuildingGroupBuilder ShowingInSpeedOverview();

        IBuildingGroupBuilder NotShowingInSpeedOverview();

        IBuildingGroupBuilder DisplayableAsReward();

        IBuildingGroupBuilder NotDisplayableAsReward();

        IBuildingGroupBuilder SkippingReplacementConnectorChecks();

        IBuildingGroupBuilder NotSkippingReplacementConnectorChecks();

        IBuildingGroupBuilder WithConnectionMultiplier(int autoAttractScore);

        IBuildingGroupBuilder WithPipetteOverride(BuildingDefinitionGroupId overrideGroup);

        IBuildingGroupBuilder WithPlacementIndicator<TPlacementIndicator>()
            where TPlacementIndicator : IBuildingPlacementIndicator;

        IBuildingGroupBuilder WithPlacementRequirements(IEnumerable<IBuildingPlacementRequirement> requirements);

        IBuildingGroupBuilder WithCustomStructureOverview(MetaStructureOverview structureOverview);

        IBuildingGroupBuilder WithDefaultStructureOverview();

        BuildingDefinitionGroup BuildAndRegister(GameBuildings gameBuildings);
    }

    public interface IBuildingGroupBaseDataProvider
    {
        public BuildingDefinitionGroupId GroupId { get; }
    }
}
