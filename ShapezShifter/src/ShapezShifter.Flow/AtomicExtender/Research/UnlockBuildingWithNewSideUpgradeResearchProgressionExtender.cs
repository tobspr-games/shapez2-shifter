using ShapezShifter.Flow.Atomic;

namespace ShapezShifter.Flow.Research
{
    public class UnlockBuildingWithNewSideUpgradeResearchProgressionExtender : IBuildingResearchProgressionExtender
    {
        private readonly IPresentableUnlockableSideUpgradeBuilder SideUpgradeBuilder;

        public UnlockBuildingWithNewSideUpgradeResearchProgressionExtender(
            IPresentableUnlockableSideUpgradeBuilder sideUpgradeBuilder)
        {
            SideUpgradeBuilder = sideUpgradeBuilder;
        }

        public void ExtendResearch(string scenarioId, ResearchProgression researchProgression,
            BuildingDefinitionGroupId groupId)
        {
            ResearchSideUpgrade sideUpgrade = SideUpgradeBuilder.Build(scenarioId, researchProgression);
            sideUpgrade.Rewards.Add(new ResearchRewardBuildingGroup(groupId));
        }
    }
}