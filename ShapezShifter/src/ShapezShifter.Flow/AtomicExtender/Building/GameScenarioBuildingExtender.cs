using Core.Events;
using ShapezShifter.Flow.Research;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    internal class GameScenarioBuildingExtender : IGameScenarioRewirer, IChainableRewirer
    {
        private readonly ScenarioSelector ScenarioFilter;
        private readonly IBuildingResearchProgressionExtender ProgressionExtender;
        private readonly BuildingDefinitionGroupId GroupId;

        public GameScenarioBuildingExtender(ScenarioSelector scenarioFilter,
            IBuildingResearchProgressionExtender progressionExtender, BuildingDefinitionGroupId groupId)
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