using System.Linq;
using ShapezShifter.Flow.Atomic;

namespace ShapezShifter.Flow.Research
{
    public class UnlockIslandWithExistingSideUpgradeResearchProgressionExtender : IIslandResearchProgressionExtender
    {
        private readonly ISideUpgradeSelector SideUpgradeSelector;

        public UnlockIslandWithExistingSideUpgradeResearchProgressionExtender(
            ISideUpgradeSelector sideUpgradeSelector)
        {
            SideUpgradeSelector = sideUpgradeSelector;
        }

        public void ExtendResearch(string scenarioId, ResearchProgression researchProgression,
            IslandDefinitionGroupId groupId)
        {
            ResearchSideUpgrade sideUpgrade = SideUpgradeSelector.Select(scenarioId, researchProgression.SideUpgrades);
            sideUpgrade.Rewards = sideUpgrade.Rewards
               .Append(new ResearchRewardIslandGroup(new SerializedResearchRewardIslandGroup(groupId.Name)))
               .ToList();
        }
    }
}