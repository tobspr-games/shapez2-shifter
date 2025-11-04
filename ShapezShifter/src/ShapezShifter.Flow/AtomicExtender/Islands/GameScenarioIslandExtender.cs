using Core.Events;
using ShapezShifter.Flow.Research;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    internal class GameScenarioIslandExtender : IGameScenarioRewirer, IChainableRewirer
    {
        private readonly ScenarioSelector ScenarioFilter;
        private readonly IIslandResearchProgressionExtender ProgressionExtender;
        private readonly IslandDefinitionGroupId GroupId;

        public GameScenarioIslandExtender(ScenarioSelector scenarioFilter,
            IIslandResearchProgressionExtender progressionExtender, IslandDefinitionGroupId groupId)
        {
            ScenarioFilter = scenarioFilter;
            ProgressionExtender = progressionExtender;
            GroupId = groupId;
        }

        public GameScenario ModifyGameScenario(GameScenario gameScenario)
        {
            if (!ScenarioFilter.Invoke(gameScenario))
            {
                return gameScenario;
            }

            ProgressionExtender.ExtendResearch(gameScenario.UniqueId, gameScenario.Progression, GroupId);
            _AfterExtensionApplied.Invoke();
            return gameScenario;
        }

        public IEvent AfterHijack => _AfterExtensionApplied;
        private readonly MultiRegisterEvent _AfterExtensionApplied = new();
    }
}