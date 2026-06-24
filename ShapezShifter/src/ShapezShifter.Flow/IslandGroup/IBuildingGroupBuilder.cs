using Game.Core.Content.Islands;

namespace ShapezShifter.Flow
{
    public interface IIslandGroupBuilder : IIslandGroupBaseDataProvider
    {
        IIslandGroupBuilder Removable();

        IIslandGroupBuilder NotRemovable();

        IIslandGroupBuilder Selectable();

        IIslandGroupBuilder NotSelectable();

        IIslandGroupBuilder Buildable();

        IIslandGroupBuilder NotBuildable();

        IIslandGroupBuilder AllowedOnNonFilledTiles();

        IIslandGroupBuilder NotAllowedOnNonFilledTiles();

        IIslandGroupBuilder AllowedOnNotches();

        IIslandGroupBuilder NotAllowedOnNotches();

        IIslandGroupBuilder AutoConnected();

        IIslandGroupBuilder NotAutoConnected();

        IIslandGroupBuilder AutoRotated();

        IIslandGroupBuilder NotAutoRotated();

        IIslandGroupBuilder AllowedToBeReplacedWithoutForce();

        IIslandGroupBuilder NotAllowedToBeReplacedWithoutForce();

        IIslandGroupBuilder RenderingConflictingIndicatorMeshes();

        IIslandGroupBuilder NotRenderingConflictingIndicatorMeshes();

        IIslandGroupBuilder RenderingConflictingIndicatorVisualization();

        IIslandGroupBuilder NotRenderingConflictingIndicatorVisualization();

        IIslandGroupBuilder ProducingConflictingIndicatorsAlways();

        IIslandGroupBuilder NotProducingConflictingIndicatorsAlways();

        IIslandGroupBuilder RenderingConnectorIndicators();

        IIslandGroupBuilder NotRenderingConnectorIndicator();

        IIslandGroupBuilder RenderingConnectorConflictIndicators();

        IIslandGroupBuilder NotRenderingConnectorConflictIndicator();

        IIslandGroupBuilder ShowingBeltProcessingTimeStat();

        IIslandGroupBuilder NotShowingBeltProcessingTimeStat();

        IIslandGroupBuilder ShowingIslandsPerFullBeltStat();

        IIslandGroupBuilder NotShowingIslandsPerFullBeltStat();

        IIslandGroupBuilder DisplayableAsReward();

        IIslandGroupBuilder NotDisplayableAsReward();

        IIslandGroupBuilder SkippingReplacementConnectorChecks();

        IIslandGroupBuilder NotSkippingReplacementConnectorChecks();

        IIslandGroupBuilder WithConnectionMultiplier(int autoAttractScore);

        IIslandGroupBuilder WithPipetteOverride(IslandDefinitionGroupId overrideGroup);

        IIslandGroupBuilder WithoutPipetteOverride();

        IIslandGroupBuilder WithoutPlacementIndicators();

        IIslandGroupBuilder WithPlacementRequirements();

        IIslandGroupBuilder WithoutPlacementRequirements();

        IIslandGroupBuilder WithoutThroughputDisplayHelper();

        IslandDefinitionGroup BuildAndRegister(GameIslands gameIslands);
    }

    public interface IIslandGroupBaseDataProvider
    {
        public IslandDefinitionGroupId GroupId { get; }
    }
}
