using System.Linq;
using Game.Core.Content.Buildings;
using ShapezShifter.Flow.Atomic;

namespace ShapezShifter.Flow.Research
{
    public class UnlockBuildingWithExistingSideUpgradeResearchProgressionExtender : IBuildingResearchProgressionExtender
    {
        private readonly ISideUpgradeSelector SideUpgradeSelector;

        public UnlockBuildingWithExistingSideUpgradeResearchProgressionExtender(
            ISideUpgradeSelector sideUpgradeSelector)
        {
            SideUpgradeSelector = sideUpgradeSelector;
        }

        public void ExtendResearch(
            ScenarioId scenarioId,
            ResearchProgression researchProgression,
            BuildingDefinitionGroupId groupId)
        {
            ResearchSideUpgrade sideUpgrade = SideUpgradeSelector.Select(
                scenarioId: scenarioId,
                progression: researchProgression);
            sideUpgrade.Rewards = sideUpgrade.Rewards.Append(new ResearchRewardBuildingGroup(groupId)).ToList();
        }
    }
}
