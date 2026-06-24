using Game.Core.Content.Islands;
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

        public void ExtendResearch(
            ScenarioId scenarioId,
            ResearchProgression researchProgression,
            IslandDefinitionGroupId groupId)
        {
            ResearchSideUpgrade sideUpgrade = SideUpgradeBuilder.Build(
                scenarioId: scenarioId,
                progression: researchProgression);
            sideUpgrade.Rewards.Add(
                new ResearchRewardIslandGroup(new SerializedResearchRewardIslandGroup(groupId.Name)));
        }
    }
}
