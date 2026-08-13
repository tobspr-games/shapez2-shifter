using Core.Events;
using Game.Core.Content.Buildings;
using ShapezShifter.Flow.Research;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    internal class GameScenarioBuildingExtender : IGameScenarioRewirer, IChainableRewirer
    {
        private readonly ScenarioSelector ScenarioFilter;
        private readonly IBuildingResearchProgressionExtender ProgressionExtender;
        private readonly BuildingDefinitionGroupId GroupId;

        public GameScenarioBuildingExtender(
            ScenarioSelector scenarioFilter,
            IBuildingResearchProgressionExtender progressionExtender,
            BuildingDefinitionGroupId groupId)
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

            ProgressionExtender.ExtendResearch(
                scenarioId: gameScenario.UniqueId,
                researchProgression: gameScenario.Progression,
                groupId: GroupId);
            _AfterExtensionApplied.Invoke();
            return gameScenario;
        }

        public IEvent AfterHijack
        {
            get { return _AfterExtensionApplied; }
        }

        private readonly MultiRegisterEvent _AfterExtensionApplied = new();
    }
}
