using System.Linq;
using Game.Core.Content.Islands;

namespace ShapezShifter.Flow.Research
{
    public class UnlockIslandWithMilestoneResearchProgressionExtender : IIslandResearchProgressionExtender
    {
        private readonly IMilestoneSelector MilestoneSelector;

        public UnlockIslandWithMilestoneResearchProgressionExtender(IMilestoneSelector milestoneSelector)
        {
            MilestoneSelector = milestoneSelector;
        }

        public void ExtendResearch(
            ScenarioId scenarioId,
            ResearchProgression researchProgression,
            IslandDefinitionGroupId groupId)
        {
            ResearchLevel level = MilestoneSelector.Select(
                scenarioId: scenarioId,
                milestones: researchProgression.Levels);
            level.Rewards = level.Rewards
                                 .Append(
                                      new ResearchRewardIslandGroup(
                                          new SerializedResearchRewardIslandGroup(groupId.Name)))
                                 .ToList();
        }
    }
}
