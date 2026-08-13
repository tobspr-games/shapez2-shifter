using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core.Research;
using ShapezShifter.Flow.Research;

namespace ShapezShifter.Flow.Atomic
{
    public static class SideUpgrade
    {
        public static ISideUpgradeBuilder New()
        {
            return new SideUpgradeBuilder();
        }
    }

    public class SideUpgradeBuilder
        : ISideUpgradeBuilder,
          IPresentableSideUpgradeBuilder,
          ICostingSideUpgradeBuilder,
          IPresentableUnlockableSideUpgradeBuilder
    {
        private SideUpgradePresentationData SideUpgradePresentationData;
        private IResearchCost[] Costs;
        private ResearchMechanicId[] Mechanics;
        private ResearchUpgradeId[] Upgrades;
        private IResearchReward[] AdditionalRewards;
        private ISideUpgradeSelector CopyRequirementsSideUpgradeSelector;

        internal SideUpgradeBuilder() { }

        public IPresentableSideUpgradeBuilder WithPresentationData(
            SideUpgradePresentationData sideUpgradePresentationData)
        {
            SideUpgradePresentationData = sideUpgradePresentationData;
            return this;
        }

        public ICostingSideUpgradeBuilder WithCost(IEnumerable<IResearchCost> costs)
        {
            Costs = costs.ToArray();
            return this;
        }

        public IPresentableUnlockableSideUpgradeBuilder CopyingRequirements(ISideUpgradeSelector sideUpgradeSelector)
        {
            CopyRequirementsSideUpgradeSelector = sideUpgradeSelector;
            return this;
        }

        public IPresentableUnlockableSideUpgradeBuilder WithCustomRequirements(
            IEnumerable<ResearchMechanicId> mechanics,
            IEnumerable<ResearchUpgradeId> upgrades)
        {
            Mechanics = mechanics.ToArray();
            Upgrades = upgrades.ToArray();
            return this;
        }

        public IPresentableUnlockableSideUpgradeBuilder WithoutCustomRequirements()
        {
            return WithCustomRequirements(
                mechanics: Array.Empty<ResearchMechanicId>(),
                upgrades: Array.Empty<ResearchUpgradeId>());
        }

        public IPresentableUnlockableSideUpgradeBuilder WithAdditionalRewards(
            IEnumerable<IResearchReward> additionalRewards)
        {
            AdditionalRewards = additionalRewards.ToArray();
            return this;
        }

        public ResearchSideUpgrade Build(ScenarioId scenarioId, ResearchProgression progression)
        {
            var mechanics = Mechanics;
            var upgrades = Upgrades;

            if (CopyRequirementsSideUpgradeSelector != null)
            {
                ResearchSideUpgrade sideUpgrade = CopyRequirementsSideUpgradeSelector.Select(
                    scenarioId: scenarioId,
                    progression: progression);
                mechanics = sideUpgrade.RequiredMechanics.ToArray();
                upgrades = sideUpgrade.RequiredUpgrades.ToArray();
            }

            ResearchSideUpgrade upgrade = new(
                id: SideUpgradePresentationData.Id,
                title: SideUpgradePresentationData.Title,
                description: SideUpgradePresentationData.Description,
                videoId: SideUpgradePresentationData.VideoId,
                imageId: SideUpgradePresentationData.PreviewImageId,
                rewards: AdditionalRewards ?? Array.Empty<IResearchReward>(),
                costs: Costs,
                requiredUpgrades: upgrades,
                requiredMechanics: mechanics,
                hidden: SideUpgradePresentationData.Hidden,
                category: SideUpgradePresentationData.Category);

            progression._SideUpgrades.Add(upgrade);
            progression._ShopItems.Add(upgrade);

            progression._AllUpgrades.Add(upgrade);
            progression._UpgradesById[upgrade.Id] = upgrade;

            if (!progression._SideUpgradeCategories.Contains(upgrade.Category))
                progression._SideUpgradeCategories.Add(upgrade.Category);

            return upgrade;
        }
    }

    public interface ISideUpgradeBuilder
    {
        IPresentableSideUpgradeBuilder WithPresentationData(SideUpgradePresentationData sideUpgradePresentationData);
    }

    public interface IPresentableSideUpgradeBuilder
    {
        ICostingSideUpgradeBuilder WithCost(IEnumerable<IResearchCost> costs);
    }

    public interface ICostingSideUpgradeBuilder
    {
        IPresentableUnlockableSideUpgradeBuilder CopyingRequirements(ISideUpgradeSelector sideUpgradeSelector);

        IPresentableUnlockableSideUpgradeBuilder WithCustomRequirements(
            IEnumerable<ResearchMechanicId> mechanics,
            IEnumerable<ResearchUpgradeId> upgrades);

        IPresentableUnlockableSideUpgradeBuilder WithoutCustomRequirements();
    }

    public interface IPresentableUnlockableSideUpgradeBuilder
    {
        IPresentableUnlockableSideUpgradeBuilder WithAdditionalRewards(IEnumerable<IResearchReward> additionalRewards);

        ResearchSideUpgrade Build(ScenarioId scenarioId, ResearchProgression progression);
    }
}
