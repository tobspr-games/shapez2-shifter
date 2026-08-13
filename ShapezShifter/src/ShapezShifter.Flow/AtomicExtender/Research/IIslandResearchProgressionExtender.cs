using Game.Core.Content.Islands;

namespace ShapezShifter.Flow.Research
{
    public interface IIslandResearchProgressionExtender
    {
        void ExtendResearch(
            ScenarioId scenarioId,
            ResearchProgression researchProgression,
            IslandDefinitionGroupId groupId);
    }
}
