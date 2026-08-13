using Game.Core.Content.Buildings;

namespace ShapezShifter.Flow.Research
{
    public interface IBuildingResearchProgressionExtender
    {
        void ExtendResearch(
            ScenarioId scenarioId,
            ResearchProgression researchProgression,
            BuildingDefinitionGroupId groupId);
    }
}
