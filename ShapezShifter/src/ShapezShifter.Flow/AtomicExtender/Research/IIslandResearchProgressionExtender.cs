namespace ShapezShifter.Flow.Research
{
    public interface IIslandResearchProgressionExtender
    {
        void ExtendResearch(string scenarioId, ResearchProgression researchProgression,
            IslandDefinitionGroupId groupId);
    }
}