using Core.Events;
using Game.Core.Content.Islands;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    public class IslandsExtender : IIslandsRewirer, IChainableRewirer<IslandDefinition>
    {
        private readonly IIslandBuilder IslandBuilder;
        private readonly IIslandGroupBuilder IslandGroupBuilder;

        public IEvent<IslandDefinition> AfterHijack
        {
            get { return _AfterExtensionApplied; }
        }

        private readonly MultiRegisterEvent<IslandDefinition> _AfterExtensionApplied = new();

        public IslandsExtender(IIslandBuilder islandBuilder, IIslandGroupBuilder islandGroupBuilder)
        {
            IslandBuilder = islandBuilder;
            IslandGroupBuilder = islandGroupBuilder;
        }

        public GameIslands ModifyGameIslands(
            IslandDefinitionFactory factory,
            AuthoringIslands metaIslands,
            GameIslands gameIslands)
        {
            IslandDefinitionGroup islandGroup = IslandGroupBuilder.BuildAndRegister(gameIslands);
            IslandDefinition island = IslandBuilder.BuildAndRegister(group: islandGroup, gameIslands: gameIslands);

            IslandDefinitionGroupId groupId = islandGroup.Id;
            string groupName = islandGroup.Id.Name;

            factory.IslandGroupDefinitionFactory.GroupIdToSerialMap.Add(key: groupId, value: groupName);
            factory.IslandGroupDefinitionFactory.GroupSerialToIdMap.Add(key: groupName, value: groupId);
            factory.IslandGroupDefinitionFactory.GroupImplementationMap.Add(key: groupId, value: islandGroup);

            _AfterExtensionApplied.Invoke(island);
            return gameIslands;
        }
    }
}
