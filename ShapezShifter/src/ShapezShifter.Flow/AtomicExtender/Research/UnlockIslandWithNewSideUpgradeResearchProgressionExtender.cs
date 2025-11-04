using ShapezShifter.Flow.Atomic;

namespace ShapezShifter.Flow.Research
{
    public class UnlockIslandWithNewSideUpgradeResearchProgressionExtender : IIslandResearchProgressionExtender
    {
        private readonly IPresentableUnlockableSideUpgradeBuilder SideUpgradeBuilder;

        public UnlockIslandWithNewSideUpgradeResearchProgressionExtender(
            IPresentableUnlockableSideUpgradeBuilder sideUpgradeBuilder)
        {
            SideUpgradeBuilder = sideUpgradeBuilder;
        }

        public void ExtendResearch(string scenarioId, ResearchProgression researchProgression,
            IslandDefinitionGroupId groupId)
        {
            ResearchSideUpgrade sideUpgrade = SideUpgradeBuilder.Build(scenarioId, researchProgression);
            sideUpgrade.Rewards.Add(
                new ResearchRewardIslandGroup(new SerializedResearchRewardIslandGroup(groupId.Name)));
        }
    }
}