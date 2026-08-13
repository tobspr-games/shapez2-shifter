using System.Collections.Generic;
using Game.Core.Coordinates;

namespace ShapezShifter.Flow.Atomic
{
    public static class BuildingConnectors
    {
        public static ISingleTileConnectorDataBuilder SingleTile()
        {
            return new SingleTileBuildingConnectorDataBuilder();
        }

        public static IMultiTileConnectorDataBuilder MultiTile()
        {
            return new MultiTileBuildingConnectorDataBuilder();
        }
    }

    public class SingleTileBuildingConnectorDataBuilder : ISingleTileConnectorDataBuilder
    {
        private readonly List<BuildingBaseIO> BuildingConnectors = new();

        public ISingleTileConnectorDataBuilder AddShapeInput(ShapeConnectorConfig shapeConnectorConfig)
        {
            BuildingConnectors.Add(
                new BuildingItemInput
                {
                    Position_L = TileVector.Zero,
                    Direction_L = shapeConnectorConfig.Direction.Value,
                    StandType = shapeConnectorConfig.StandType,
                    IOType = shapeConnectorConfig.CapsType,
                    Seperators = shapeConnectorConfig.Separators
                });
            return this;
        }

        public ISingleTileConnectorDataBuilder AddShapeOutput(ShapeConnectorConfig shapeConnectorConfig)
        {
            BuildingConnectors.Add(
                new BuildingItemOutput
                {
                    Position_L = TileVector.Zero,
                    Direction_L = shapeConnectorConfig.Direction.Value,
                    StandType = shapeConnectorConfig.StandType,
                    IOType = shapeConnectorConfig.CapsType,
                    Seperators = shapeConnectorConfig.Separators
                });
            return this;
        }

        public ISingleTileConnectorDataBuilder AddWireInput(WireConnectorConfig wireConnectorConfig)
        {
            BuildingConnectors.Add(
                new BuildingSignalInput
                {
                    Position_L = TileVector.Zero,
                    Direction_L = wireConnectorConfig.Direction.Value,
                    _IOType = wireConnectorConfig.IoType,
                    TileDirection = wireConnectorConfig.Direction
                });
            return this;
        }

        public ISingleTileConnectorDataBuilder AddWireOutput(WireConnectorConfig wireConnectorConfig)
        {
            BuildingConnectors.Add(
                new BuildingSignalOutput
                {
                    Position_L = TileVector.Zero,
                    Direction_L = wireConnectorConfig.Direction.Value,
                    _IOType = wireConnectorConfig.IoType,
                    TileDirection = wireConnectorConfig.Direction
                });
            return this;
        }

        public ISingleTileConnectorDataBuilder AddFluidInput(FluidConnectorConfig fluidConnectorConfig)
        {
            BuildingConnectors.Add(
                new BuildingFluidInput
                {
                    Position_L = TileVector.Zero,
                    Direction_L = fluidConnectorConfig.Direction.Value,
                    _IOType = fluidConnectorConfig.IoType,
                    TileDirection = fluidConnectorConfig.Direction
                });
            return this;
        }

        public ISingleTileConnectorDataBuilder AddFluidOutput(FluidConnectorConfig fluidConnectorConfig)
        {
            BuildingConnectors.Add(
                new BuildingFluidOutput
                {
                    Position_L = TileVector.Zero,
                    Direction_L = fluidConnectorConfig.Direction.Value,
                    _IOType = fluidConnectorConfig.IoType,
                    TileDirection = fluidConnectorConfig.Direction
                });
            return this;
        }

        public IBuildingConnectorData Build()
        {
            TileVector[] tiles = { TileVector.Zero };

            LocalTileBounds tileBounds = new(min: TileVector.Zero, max: TileVector.Zero);

            TileDimensions tileDimensions = tileBounds.Dimensions;
            LocalVector center = LocalVector.Lerp(
                a: (LocalVector)tileBounds.Min,
                b: (LocalVector)tileBounds.Max,
                t: 0.5f);

            return new BuildingConnectorData(
                allInputs: BuildingConnectors,
                tiles: tiles,
                tileBounds: tileBounds,
                tileBoundsCenter: center,
                tileDimensions: tileDimensions);
        }
    }

    public interface ISingleTileConnectorDataBuilder
    {
        ISingleTileConnectorDataBuilder AddShapeInput(ShapeConnectorConfig shapeConnectorConfig);

        ISingleTileConnectorDataBuilder AddShapeOutput(ShapeConnectorConfig shapeConnectorConfig);

        ISingleTileConnectorDataBuilder AddWireInput(WireConnectorConfig wireConnectorConfig);

        ISingleTileConnectorDataBuilder AddWireOutput(WireConnectorConfig wireConnectorConfig);

        ISingleTileConnectorDataBuilder AddFluidInput(FluidConnectorConfig fluidConnectorConfig);

        ISingleTileConnectorDataBuilder AddFluidOutput(FluidConnectorConfig fluidConnectorConfig);

        IBuildingConnectorData Build();
    }

    public class MultiTileBuildingConnectorDataBuilder : IMultiTileConnectorDataBuilder
    {
        private readonly List<BuildingBaseIO> BuildingConnectors = new();
        private readonly HashSet<TileVector> BuildingTiles = new();

        public IMultiTileConnectorDataBuilder AddTile(TileVector tile)
        {
            BuildingTiles.Add(tile);
            return this;
        }

        public IMultiTileConnectorDataBuilder AddShapeInput(TileVector tile, ShapeConnectorConfig shapeConnectorConfig)
        {
            BuildingTiles.Add(tile);
            BuildingConnectors.Add(new BuildingItemInput
            {
                Position_L = tile,
                Direction_L = shapeConnectorConfig.Direction.Value,
                StandType = shapeConnectorConfig.StandType,
                IOType = shapeConnectorConfig.CapsType,
                Seperators = shapeConnectorConfig.Separators
            });
            return this;
        }

        public IMultiTileConnectorDataBuilder AddShapeOutput(TileVector tile, ShapeConnectorConfig shapeConnectorConfig)
        {
            BuildingTiles.Add(tile);
            BuildingConnectors.Add(new BuildingItemOutput
            {
                Position_L = tile,
                Direction_L = shapeConnectorConfig.Direction.Value,
                StandType = shapeConnectorConfig.StandType,
                IOType = shapeConnectorConfig.CapsType,
                Seperators = shapeConnectorConfig.Separators
            });
            return this;
        }

        public IMultiTileConnectorDataBuilder AddWireInput(TileVector tile, WireConnectorConfig wireConnectorConfig)
        {
            BuildingTiles.Add(tile);
            BuildingConnectors.Add(new BuildingSignalInput
            {
                Position_L = tile,
                Direction_L = wireConnectorConfig.Direction.Value,
                _IOType = wireConnectorConfig.IoType,
                TileDirection = wireConnectorConfig.Direction
            });
            return this;
        }

        public IMultiTileConnectorDataBuilder AddWireOutput(TileVector tile, WireConnectorConfig wireConnectorConfig)
        {
            BuildingTiles.Add(tile);
            BuildingConnectors.Add(new BuildingSignalOutput
            {
                Position_L = tile,
                Direction_L = wireConnectorConfig.Direction.Value,
                _IOType = wireConnectorConfig.IoType,
                TileDirection = wireConnectorConfig.Direction
            });
            return this;
        }

        public IMultiTileConnectorDataBuilder AddFluidInput(TileVector tile, FluidConnectorConfig fluidConnectorConfig)
        {
            BuildingTiles.Add(tile);
            BuildingConnectors.Add(new BuildingFluidInput
            {
                Position_L = tile,
                Direction_L = fluidConnectorConfig.Direction.Value,
                _IOType = fluidConnectorConfig.IoType,
                TileDirection = fluidConnectorConfig.Direction
            });
            return this;
        }

        public IMultiTileConnectorDataBuilder AddFluidOutput(TileVector tile, FluidConnectorConfig fluidConnectorConfig)
        {
            BuildingTiles.Add(tile);
            BuildingConnectors.Add(new BuildingFluidOutput
            {
                Position_L = tile,
                Direction_L = fluidConnectorConfig.Direction.Value,
                _IOType = fluidConnectorConfig.IoType,
                TileDirection = fluidConnectorConfig.Direction
            });
            return this;
        }

        public IBuildingConnectorData Build()
        {
            TileVector[] tilesArray = new TileVector[BuildingTiles.Count];
            BuildingTiles.CopyTo(tilesArray);

            int minX = int.MaxValue;
            int minY = int.MaxValue;
            short minZ = short.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;
            short maxZ = short.MinValue;
            foreach (TileVector tile in BuildingTiles)
            {
                if (tile.x < minX) minX = tile.x;
                if (tile.y < minY) minY = tile.y;
                if (tile.z < minZ) minZ = tile.z;
                if (tile.x > maxX) maxX = tile.x;
                if (tile.y > maxY) maxY = tile.y;
                if (tile.z > maxZ) maxZ = tile.z;
            }

            TileVector min = new(minX, minY, minZ);
            TileVector max = new(maxX, maxY, maxZ);
            LocalTileBounds tileBounds = new(min, max);
            TileDimensions tileDimensions = tileBounds.Dimensions;
            LocalVector center = LocalVector.Lerp(
                a: (LocalVector)tileBounds.Min,
                b: (LocalVector)tileBounds.Max,
                t: 0.5f);

            return new BuildingConnectorData(
                allInputs: BuildingConnectors,
                tiles: tilesArray,
                tileBounds: tileBounds,
                tileBoundsCenter: center,
                tileDimensions: tileDimensions);
        }
    }
    public interface IMultiTileConnectorDataBuilder
    {
        IMultiTileConnectorDataBuilder AddTile(TileVector tile);

        IMultiTileConnectorDataBuilder AddShapeInput(TileVector tile, ShapeConnectorConfig config);
        IMultiTileConnectorDataBuilder AddShapeOutput(TileVector tile, ShapeConnectorConfig config);
        IMultiTileConnectorDataBuilder AddWireInput(TileVector tile, WireConnectorConfig config);
        IMultiTileConnectorDataBuilder AddWireOutput(TileVector tile, WireConnectorConfig config);
        IMultiTileConnectorDataBuilder AddFluidInput(TileVector tile, FluidConnectorConfig config);
        IMultiTileConnectorDataBuilder AddFluidOutput(TileVector tile, FluidConnectorConfig config);

        IBuildingConnectorData Build();
    }
}
