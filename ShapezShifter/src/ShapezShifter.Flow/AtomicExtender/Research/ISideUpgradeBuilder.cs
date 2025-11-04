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

    public class SideUpgradeBuilder : ISideUpgradeBuilder, IPresentableSideUpgradeBuilder, ICostingSideUpgradeBuilder,
        IPresentableUnlockableSideUpgradeBuilder
    {
        private SideUpgradePresentationData SideUpgradePresentationData;
        private IResearchCost[] Costs;
        private ResearchMechanicId[] Mechanics;
        private ResearchUpgradeId[] Upgrades;
        private IResearchReward[] AdditionalRewards;
        private ISideUpgradeSelector CopyRequirementsSideUpgradeSelector;

        internal SideUpgradeBuilder()
        {
        }

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
            IEnumerable<ResearchMechanicId> mechanics, IEnumerable<ResearchUpgradeId> upgrades)
        {
            Mechanics = mechanics.ToArray();
            Upgrades = upgrades.ToArray();
            return this;
        }

        public IPresentableUnlockableSideUpgradeBuilder WithAdditionalRewards(
            IEnumerable<IResearchReward> additionalRewards)
        {
            AdditionalRewards = additionalRewards.ToArray();
            return this;
        }

        public ResearchSideUpgrade Build(string scenarioId, ResearchProgression progression)
        {
            ResearchMechanicId[] mechanics = Mechanics;
            ResearchUpgradeId[] upgrades = Upgrades;

            if (CopyRequirementsSideUpgradeSelector != null)
            {
                ResearchSideUpgrade sideUpgrade =
                    CopyRequirementsSideUpgradeSelector.Select(scenarioId, progression.SideUpgrades);
                mechanics = sideUpgrade.RequiredMechanics.ToArray();
                upgrades = sideUpgrade.RequiredUpgrades.ToArray();
            }

            ResearchSideUpgrade upgrade = new(SideUpgradePresentationData.Id,
                SideUpgradePresentationData.Title,
                SideUpgradePresentationData.Description,
                SideUpgradePresentationData.VideoId,
                SideUpgradePresentationData.PreviewImageId,
                AdditionalRewards ?? Array.Empty<IResearchReward>(),
                Costs,
                upgrades,
                mechanics,
                SideUpgradePresentationData.Hidden,
                SideUpgradePresentationData.Category);

            progression._SideUpgrades.Add(upgrade);

            progression._AllUpgrades.Add(upgrade);
            progression._UpgradesById[upgrade.Id] = upgrade;

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

        IPresentableUnlockableSideUpgradeBuilder WithCustomRequirements(IEnumerable<ResearchMechanicId> mechanics,
            IEnumerable<ResearchUpgradeId> upgrades);
    }


    public interface IPresentableUnlockableSideUpgradeBuilder
    {
        IPresentableUnlockableSideUpgradeBuilder WithAdditionalRewards(IEnumerable<IResearchReward> additionalRewards);
        ResearchSideUpgrade Build(string scenarioId, ResearchProgression progression);
    }
}