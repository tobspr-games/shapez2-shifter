using System.Linq;
using Game.Core.Content.Islands;
using ShapezShifter.Flow.Atomic;

namespace ShapezShifter.Flow.Research
{
    public class UnlockIslandWithExistingSideUpgradeResearchProgressionExtender : IIslandResearchProgressionExtender
    {
        private readonly ISideUpgradeSelector SideUpgradeSelector;

        public UnlockIslandWithExistingSideUpgradeResearchProgressionExtender(ISideUpgradeSelector sideUpgradeSelector)
        {
            SideUpgradeSelector = sideUpgradeSelector;
        }

        public void ExtendResearch(
            ScenarioId scenarioId,
            ResearchProgression researchProgression,
            IslandDefinitionGroupId groupId)
        {
            ResearchSideUpgrade sideUpgrade = SideUpgradeSelector.Select(
                scenarioId: scenarioId,
                progression: researchProgression);
            sideUpgrade.Rewards = sideUpgrade.Rewards.Append(
                                                  new ResearchRewardIslandGroup(
                                                      new SerializedResearchRewardIslandGroup(groupId.Name)))
                                             .ToList();
        }
    }
}
