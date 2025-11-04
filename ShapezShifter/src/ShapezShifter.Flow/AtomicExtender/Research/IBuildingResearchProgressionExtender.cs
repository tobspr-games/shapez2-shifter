namespace ShapezShifter.Flow.Research
{
    public interface IBuildingResearchProgressionExtender
    {
        void ExtendResearch(string scenarioId, ResearchProgression researchProgression,
            BuildingDefinitionGroupId groupId);
    }
}