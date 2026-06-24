using System.Linq;
using Game.Core.Content.Buildings;

namespace ShapezShifter.Flow.Research
{
    public class UnlockBuildingWithMilestoneResearchProgressionExtender : IBuildingResearchProgressionExtender
    {
        private readonly IMilestoneSelector MilestoneSelector;

        public UnlockBuildingWithMilestoneResearchProgressionExtender(IMilestoneSelector milestoneSelector)
        {
            MilestoneSelector = milestoneSelector;
        }

        public void ExtendResearch(
            ScenarioId scenarioId,
            ResearchProgression researchProgression,
            BuildingDefinitionGroupId groupId)
        {
            ResearchLevel level = MilestoneSelector.Select(
                scenarioId: scenarioId,
                milestones: researchProgression.Levels);
            level.Rewards = level.Rewards.Append(new ResearchRewardBuildingGroup(groupId)).ToList();
        }
    }
}
