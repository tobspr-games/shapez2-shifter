using System;
using System.Collections.Generic;
using Core.Localization;
using Game.Core.Content.Islands;
using Game.Core.Research;
using UnityEngine;

namespace ShapezShifter.Flow
{
    public class IslandGroupBuilder
        : IIdentifiableIslandGroupBuilder,
          IIdentifiableAndTitledIslandGroupBuilder,
          IIdentifiableTitledAndDescribedIslandGroupBuilder,
          IIdentifiableAndPresentableIslandGroupBuilder,
          IIdentifiablePresentableAndCategorizedIslandGroupBuilder,
          IIslandGroupBuilder
    {
        internal IslandGroupBuilder(IslandDefinitionGroupId groupId)
        {
            GroupId = groupId;
        }

        public IslandDefinitionGroupId GroupId { get; }
        private Sprite Icon;
        private IText Title;
        private IText Description;
        private bool IsTransportIsland;
        private DefaultPreferredPlacementMode DefaultPreferredPlacement;
        private bool IsRemovable = true;
        private bool IsSelectable = true;
        private bool IsBuildable = true;
        private bool AllowPlaceOnNonFilledTiles;
        private IslandDefinitionGroupId PipetteOverrideId;
        private bool AllowPlaceOnNotch;
        private int AutoAttractIOScoreMultiplier;
        private bool AutoConnect = true;
        private bool AutoRotateToFitStructures = true;
        private bool AllowNonForcingReplacementByOtherIslands;
        private bool ShouldSkipReplacementIOChecks;
        private bool AlwaysProducesConflictIndicators;
        private bool RenderConflictIndicatorMeshes;
        private bool RenderConflictIndicatorVisualization;
        private bool RenderConnectorIndicators;
        private bool RenderConflictingConnectorIndicators;
        private bool ShowStatBeltProcessingTime;
        private bool ShowStatIslandsPerFullBelt;
        private bool ShowInSpeedOverview;
        private bool ShowAsResearchReward;
        private UnlockableStoreContentId RequireStoreContentId;
        private WikiEntryId LinkedEntryId;
        private readonly List<Type> PlacementIndicators = new();
        private IEnumerable<IIslandPlacementRequirement> PlacementRequirements;

        private bool ShouldShowAsReward;

        public IIdentifiableAndPresentableIslandGroupBuilder WithPresentation(
            IText title,
            IText description,
            Sprite icon)
        {
            Title = title;
            Description = description;
            Icon = icon;
            return this;
        }

        public IIdentifiableAndTitledIslandGroupBuilder WithTitle(IText title)
        {
            Title = title;
            return this;
        }

        public IIdentifiableTitledAndDescribedIslandGroupBuilder WithDescription(IText description)
        {
            Description = description;
            return this;
        }

        public IIdentifiableAndPresentableIslandGroupBuilder WithIcon(Sprite icon)
        {
            Icon = icon;
            return this;
        }

        public IIdentifiableAndPresentableIslandGroupBuilder WithIcon(string filePath)
        {
            throw new NotImplementedException();
        }

        public IIdentifiableAndPresentableIslandGroupBuilder WithIcon(Texture texture)
        {
            throw new NotImplementedException();
        }

        public IIdentifiablePresentableAndCategorizedIslandGroupBuilder AsTransportableIsland()
        {
            IsTransportIsland = true;
            return this;
        }

        public IIdentifiablePresentableAndCategorizedIslandGroupBuilder AsNonTransportableIsland()
        {
            IsTransportIsland = false;
            return this;
        }

        public IIslandGroupBuilder WithPreferredPlacement(DefaultPreferredPlacementMode defaultPreferredPlacementMode)
        {
            DefaultPreferredPlacement = defaultPreferredPlacementMode;
            return this;
        }

        public IIslandGroupBuilder WithConfig(
            bool isTransportIsland,
            DefaultPreferredPlacementMode defaultPreferredPlacementMode)
        {
            IsTransportIsland = isTransportIsland;
            DefaultPreferredPlacement = defaultPreferredPlacementMode;
            return this;
        }

        public IIslandGroupBuilder Removable()
        {
            IsRemovable = true;
            return this;
        }

        public IIslandGroupBuilder NotRemovable()
        {
            IsRemovable = false;
            return this;
        }

        public IIslandGroupBuilder Selectable()
        {
            IsSelectable = true;
            return this;
        }

        public IIslandGroupBuilder NotSelectable()
        {
            IsSelectable = false;
            return this;
        }

        public IIslandGroupBuilder Buildable()
        {
            IsBuildable = true;
            return this;
        }

        public IIslandGroupBuilder NotBuildable()
        {
            IsBuildable = false;
            return this;
        }

        public IIslandGroupBuilder AllowedOnNonFilledTiles()
        {
            AllowPlaceOnNonFilledTiles = true;
            return this;
        }

        public IIslandGroupBuilder NotAllowedOnNonFilledTiles()
        {
            AllowPlaceOnNonFilledTiles = false;
            return this;
        }

        public IIslandGroupBuilder AllowedOnNotches()
        {
            AllowPlaceOnNotch = true;
            return this;
        }

        public IIslandGroupBuilder NotAllowedOnNotches()
        {
            AllowPlaceOnNotch = false;
            return this;
        }

        public IIslandGroupBuilder AutoConnected()
        {
            AutoConnect = true;
            return this;
        }

        public IIslandGroupBuilder NotAutoConnected()
        {
            AutoConnect = false;
            return this;
        }

        public IIslandGroupBuilder AutoRotated()
        {
            AutoRotateToFitStructures = true;
            return this;
        }

        public IIslandGroupBuilder NotAutoRotated()
        {
            AutoRotateToFitStructures = false;
            return this;
        }

        public IIslandGroupBuilder AllowedToBeReplacedWithoutForce()
        {
            AllowNonForcingReplacementByOtherIslands = true;
            return this;
        }

        public IIslandGroupBuilder NotAllowedToBeReplacedWithoutForce()
        {
            AllowNonForcingReplacementByOtherIslands = false;
            return this;
        }

        public IIslandGroupBuilder RenderingConflictingIndicatorMeshes()
        {
            RenderConflictIndicatorMeshes = true;
            return this;
        }

        public IIslandGroupBuilder NotRenderingConflictingIndicatorMeshes()
        {
            RenderConflictIndicatorMeshes = false;
            return this;
        }

        public IIslandGroupBuilder RenderingConflictingIndicatorVisualization()
        {
            RenderConflictIndicatorVisualization = true;
            return this;
        }

        public IIslandGroupBuilder NotRenderingConflictingIndicatorVisualization()
        {
            RenderConflictIndicatorVisualization = false;
            return this;
        }

        public IIslandGroupBuilder ProducingConflictingIndicatorsAlways()
        {
            AlwaysProducesConflictIndicators = true;
            return this;
        }

        public IIslandGroupBuilder NotProducingConflictingIndicatorsAlways()
        {
            AlwaysProducesConflictIndicators = false;
            return this;
        }

        public IIslandGroupBuilder RenderingConnectorIndicators()
        {
            throw new NotImplementedException();
        }

        public IIslandGroupBuilder NotRenderingConnectorIndicator()
        {
            throw new NotImplementedException();
        }

        public IIslandGroupBuilder RenderingConnectorConflictIndicators()
        {
            throw new NotImplementedException();
        }

        public IIslandGroupBuilder NotRenderingConnectorConflictIndicator()
        {
            throw new NotImplementedException();
        }

        public IIslandGroupBuilder ShowingBeltProcessingTimeStat()
        {
            throw new NotImplementedException();
        }

        public IIslandGroupBuilder NotShowingBeltProcessingTimeStat()
        {
            throw new NotImplementedException();
        }

        public IIslandGroupBuilder ShowingIslandsPerFullBeltStat()
        {
            throw new NotImplementedException();
        }

        public IIslandGroupBuilder NotShowingIslandsPerFullBeltStat()
        {
            throw new NotImplementedException();
        }

        public IIslandGroupBuilder DisplayableAsReward()
        {
            throw new NotImplementedException();
        }

        public IIslandGroupBuilder NotDisplayableAsReward()
        {
            throw new NotImplementedException();
        }

        public IIslandGroupBuilder SkippingReplacementConnectorChecks()
        {
            throw new NotImplementedException();
        }

        public IIslandGroupBuilder NotSkippingReplacementConnectorChecks()
        {
            throw new NotImplementedException();
        }

        public IIslandGroupBuilder WithConnectionMultiplier(int autoAttractScore)
        {
            throw new NotImplementedException();
        }

        public IIslandGroupBuilder WithPipetteOverride(IslandDefinitionGroupId overrideGroup)
        {
            throw new NotImplementedException();
        }

        public IIslandGroupBuilder WithoutPipetteOverride()
        {
            throw new NotImplementedException();
        }

        public IIslandGroupBuilder WithoutPlacementIndicators()
        {
            throw new NotImplementedException();
        }

        public IIslandGroupBuilder WithPlacementRequirements()
        {
            throw new NotImplementedException();
        }

        public IIslandGroupBuilder WithoutPlacementRequirements()
        {
            throw new NotImplementedException();
        }

        public IIslandGroupBuilder WithoutThroughputDisplayHelper()
        {
            throw new NotImplementedException();
        }

        public IslandDefinitionGroup BuildAndRegister(GameIslands gameIslands)
        {
            IslandDefinitionGroup islandGroup = new(GroupId);

            islandGroup.CustomData.Attach(
                new GroupPresentationData(
                    icon: Icon,
                    title: Title,
                    description: Description,
                    shouldShowAsReward: ShouldShowAsReward));

            ((List<IIslandDefinitionGroup>)gameIslands.AllDefinitionGroups).Add(islandGroup);

            return islandGroup;
        }

        public IIslandGroupBuilder WithConfig()
        {
            throw new NotImplementedException();
        }

        public IIdentifiableAndPresentableIslandGroupBuilder WithDescription(Sprite icon)
        {
            throw new NotImplementedException();
        }
    }
}
