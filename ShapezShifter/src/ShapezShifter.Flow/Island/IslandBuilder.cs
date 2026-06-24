using System.Collections.Generic;
using System.Linq;
using Core.Collections;
using Core.Factory;
using Game.Core.Coordinates;
using Game.Core.Rendering.Islands.PlayingField;
using Game.Core.Research;
using Unity.Mathematics;

namespace ShapezShifter.Flow
{
    internal class IslandBuilder
        : IIslandBuilder,
          IIdentifiableIslandBuilder,
          IIdentifiableCollidableIslandBuilder,
          IIdentifiableMappableIslandBuilder,
          IIdentifiableMappableConnectableIslandBuilder,
          IIdentifiableMappableConnectableInteractableIslandBuilder,
          IIdentifiableMappableConnectableInteractableCostingIslandBuilder
    {
        private readonly IslandDefinitionId DefinitionId;
        private ChunkLayoutLookup<ChunkVector, IslandChunkData> Layout;

        private IslandDefinition IslandDefinition;

        public IslandBuilder(IslandDefinitionId id)
        {
            DefinitionId = id;
        }

        public IIdentifiableCollidableIslandBuilder WithLayout(ChunkLayoutLookup<ChunkVector, IslandChunkData> layout)
        {
            Layout = layout;
            IslandDefinition = new IslandDefinition(id: DefinitionId, layout: Layout);
            IslandDefinition.CustomData.Attach(new LambdaFactory<IIslandConfiguration>(() => null));

            return this;
        }

        public IIdentifiableMappableIslandBuilder WithPerChunkColliders()
        {
            var chunks = IslandDefinition.Layout.GetChunkPositions();

            var colliders = new List<CollisionBox>();
            foreach (ChunkVector coordinate in chunks)
            {
                LocalVector center = new LocalVector(x: coordinate.x, y: coordinate.y, z: coordinate.z)
                                     * CoordinateConstants.TilesPerIslandLayer;
                var dimensions = new LocalDimension(
                    x: CoordinateConstants.TilesPerIslandLayer,
                    y: CoordinateConstants.TilesPerIslandLayer,
                    z: CoordinateConstants.TilesPerIslandLayer);
                colliders.Add(new CollisionBox(center_L: center, dimensions_L: dimensions));
            }

            IslandDefinition.CustomData.Attach(new IslandCollisionData(colliders));
            return this;
        }

        public IIdentifiableMappableIslandBuilder WithBoundingCollider()
        {
            var chunks = IslandDefinition.Layout.GetChunkPositions();

            var min = new LocalVector(x: float.MaxValue, y: float.MaxValue, z: float.MaxValue);
            var max = new LocalVector(x: float.MinValue, y: float.MinValue, z: float.MinValue);
            foreach (ChunkVector coordinate in chunks)
            {
                min = new LocalVector(
                    x: math.min(x: min.x, y: coordinate.x),
                    y: math.min(x: min.y, y: coordinate.y),
                    z: (short)math.min(x: min.z, y: coordinate.z));
                max = new LocalVector(
                    x: math.max(x: max.x, y: coordinate.x),
                    y: math.max(x: max.y, y: coordinate.y),
                    z: (short)math.max(x: max.z, y: coordinate.z));
            }

            min *= CoordinateConstants.TilesPerIslandLayer;
            max *= CoordinateConstants.TilesPerIslandLayer;

            LocalVector center = (min + max) * 0.5f;
            var dimensions = new LocalDimension((max - min).xyz);
            IslandDefinition.CustomData.Attach(
                new IslandCollisionData(
                    new CollisionBox(center_L: center, dimensions_L: dimensions).AsEnumerable().ToList()));
            return this;
        }

        public IIdentifiableMappableIslandBuilder WithCustomCollider(IEnumerable<CollisionBox> collisionBoxes)
        {
            IslandDefinition.CustomData.Attach(new IslandCollisionData(collisionBoxes.ToList()));
            return this;
        }

        public IIdentifiableMappableConnectableIslandBuilder WithConnectorData(IIslandConnectorData connectorData)
        {
            IslandDefinition.CustomData.Attach(connectorData);
            return this;
        }

        public IslandDefinition BuildAndRegister(IslandDefinitionGroup islandGroup, GameIslands gameIslands)
        {
            BindGroupToIsland(island: IslandDefinition, group: islandGroup);
            BindIslandToGroup(group: islandGroup, island: IslandDefinition);

            ((List<IIslandDefinition>)gameIslands.AllDefinitions).Add(IslandDefinition);
            gameIslands.DefinitionsById.Add(key: IslandDefinition.Id, value: IslandDefinition);

            return IslandDefinition;
        }

        private static void BindGroupToIsland(IslandDefinition island, IslandDefinitionGroup group)
        {
            var groupPresentationData = group.CustomData.Get<IPresentationData>();
            IslandPresentationData islandPresentationData = new(
                title: groupPresentationData.Title,
                description: groupPresentationData.Description,
                wikiEntryId: WikiEntryId.Empty,
                icon: groupPresentationData.Icon,
                showAsReward: groupPresentationData.ShowAsReward);

            island.CustomData.AttachOrReplace(islandPresentationData);
            island.CustomData.AttachOrReplace(group);
            island.CustomData.AttachOrReplace(group.Id);
        }

        private static void BindIslandToGroup(IslandDefinitionGroup group, IslandDefinition island)
        {
            IslandGroupCollection groupCollection = group.CustomData.TryGet(out IslandGroupCollection collection)
                ? collection
                : new IslandGroupCollection();
            groupCollection.AddIslandDefinition(island);
            group.CustomData.AttachOrReplace(groupCollection);
        }

        public IIdentifiableMappableConnectableInteractableIslandBuilder WithInteraction(
            bool flippable,
            bool canHoldBuildings,
            bool allowNonForcingReplacement = false,
            bool skipReplacementConnectorChecks = false,
            bool isTransportBuilding = false,
            bool selectable = true,
            bool buildable = true,
            bool removable = true)
        {
            IslandDefinition.CustomData.Attach(
                new IslandInteractionConfig(
                    flippable: flippable,
                    selectable: selectable,
                    playerBuildable: buildable,
                    removable: removable));
            IslandDefinition.CustomData.Attach(
                new EntityReplacementPreferenceData(
                    allowNonForcingReplacementByEntitiesInDifferentGroup: allowNonForcingReplacement,
                    isTransportBuilding: isTransportBuilding,
                    shouldSkipReplacementIOChecks: skipReplacementConnectorChecks));

            if (canHoldBuildings)
            {
                IslandDefinition.CustomData.AddFlag<CanHoldBuildingsIslandTag>();
            }

            return this;
        }

        public IIdentifiableMappableConnectableInteractableCostingIslandBuilder WithCustomChunkCost(
            ChunkLimitCurrency totalChunkCost)
        {
            IslandDefinition.CustomData.Attach(new ChunkCostProvider(totalChunkCost));
            return this;
        }

        public IIdentifiableMappableConnectableInteractableCostingIslandBuilder WithDefaultChunkCost()
        {
            int chunks = IslandDefinition.Layout.GetChunkPositions().Count;
            const int costPerChunk = 1;
            return WithCustomChunkCost(new ChunkLimitCurrency(chunks * costPerChunk));
        }

        public IIslandBuilder WithRenderingOptions(
            IChunkDrawingContextProvider drawingContextProvider,
            bool drawPlayingField)
        {
            var dictionary = new Dictionary<ChunkVector, ChunkPlatformDrawingContext>();
            foreach (ChunkVector chunkVector in IslandDefinition.Layout.GetChunkPositions())
            {
                dictionary.Add(key: chunkVector, value: drawingContextProvider.DrawingContextForChunk(chunkVector));
            }

            IslandDefinition.CustomData.Attach(new IslandFrameDrawData(dictionary));
            if (drawPlayingField)
            {
                IslandDefinition.CustomData.AddFlag<DrawPlayingFieldFlag>();
            }

            return this;
        }
    }

    public interface IIdentifiableIslandBuilder
    {
        IIdentifiableCollidableIslandBuilder WithLayout(ChunkLayoutLookup<ChunkVector, IslandChunkData> layout);
    }

    public interface IIdentifiableCollidableIslandBuilder
    {
        IIdentifiableMappableIslandBuilder WithPerChunkColliders();

        IIdentifiableMappableIslandBuilder WithBoundingCollider();

        IIdentifiableMappableIslandBuilder WithCustomCollider(IEnumerable<CollisionBox> collisionBoxes);
    }

    public interface IIdentifiableMappableIslandBuilder
    {
        IIdentifiableMappableConnectableIslandBuilder WithConnectorData(IIslandConnectorData connectorData);
    }

    public interface IIdentifiableMappableConnectableIslandBuilder
    {
        IIdentifiableMappableConnectableInteractableIslandBuilder WithInteraction(
            bool flippable,
            bool canHoldBuildings,
            bool allowNonForcingReplacement = false,
            bool skipReplacementConnectorChecks = false,
            bool isTransportBuilding = false,
            bool selectable = true,
            bool buildable = true,
            bool removable = true);
    }

    public interface IIdentifiableMappableConnectableInteractableIslandBuilder
    {
        IIdentifiableMappableConnectableInteractableCostingIslandBuilder WithCustomChunkCost(
            ChunkLimitCurrency totalChunkCost);

        IIdentifiableMappableConnectableInteractableCostingIslandBuilder WithDefaultChunkCost();
    }

    public interface IIdentifiableMappableConnectableInteractableCostingIslandBuilder
    {
        IIslandBuilder WithRenderingOptions(IChunkDrawingContextProvider drawingContextProvider, bool drawPlayingField);
    }
}
