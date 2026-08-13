using Game.Core.Content.Islands;

namespace ShapezShifter.Flow
{
    public static class IslandGroup
    {
        public static IIdentifiableIslandGroupBuilder Create(IslandDefinitionGroupId definitionGroupId)
        {
            return new IslandGroupBuilder(definitionGroupId);
        }
    }
}
