using Core.Events;
using Game.Core.Content.Islands;
using ShapezShifter.Flow.Research;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    internal class GameScenarioIslandExtender : IGameScenarioRewirer, IChainableRewirer
    {
        private readonly ScenarioSelector ScenarioFilter;
        private readonly IIslandResearchProgressionExtender ProgressionExtender;
        private readonly IslandDefinitionGroupId GroupId;

        public GameScenarioIslandExtender(
            ScenarioSelector scenarioFilter,
            IIslandResearchProgressionExtender progressionExtender,
            IslandDefinitionGroupId groupId)
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
