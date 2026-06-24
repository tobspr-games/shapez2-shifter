using Game.Core.Content.Buildings;

namespace ShapezShifter.Flow
{
    public static class BuildingGroup
    {
        public static IIdentifiableBuildingGroupBuilder Create(BuildingDefinitionGroupId definitionGroupId)
        {
            return new BuildingGroupBuilder(definitionGroupId);
        }
    }
}
