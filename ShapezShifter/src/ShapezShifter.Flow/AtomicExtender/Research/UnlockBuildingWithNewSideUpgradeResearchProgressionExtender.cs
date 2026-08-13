using Game.Core.Content.Buildings;
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

        public void ExtendResearch(
            ScenarioId scenarioId,
            ResearchProgression researchProgression,
            BuildingDefinitionGroupId groupId)
        {
            ResearchSideUpgrade sideUpgrade = SideUpgradeBuilder.Build(
                scenarioId: scenarioId,
                progression: researchProgression);
            sideUpgrade.Rewards.Add(new ResearchRewardBuildingGroup(groupId));
        }
    }
}
