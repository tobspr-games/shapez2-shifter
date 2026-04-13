using System;
using Core.Logging;
using Game.Content.Features.Predictions;
using Game.Core.Simulation;
using ShapezShifter.Flow.Research;
using ShapezShifter.Flow.Toolbar;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    public class AtomicBuildingExtender
        : IBaseBuildingExtender,
          IScenarioSelectiveBuildingExtender,
          IDefinedBuildingExtender,
          IDefinedPlaceableBuildingExtender,
          IDefinedPlaceableAccessibleBuildingExtender,
          IDefinedSimulatableBuildingExtender,
          IDefinedSimulatablePlaceableBuildingExtender,
          IAtomicBuildingExtender,
          IBuildingExtender,
          IDefinedUnlockableBuildingExtender,
          IViewModularBuildingExtender
    {
        // If each interface would have a specialized implementor, these fields could become
        // read-only. However that would create a lot of extra boiler-plate code

        private ScenarioSelector ScenarioFilter;

        private IToolbarEntryInsertLocation ToolbarEntryInsertLocation;
        private IBuildingModulesData ModulesData;
        private IBuildingBuilder BuildingBuilder;
        private IBuildingGroupBuilder BuildingGroupBuilder;
        private ISimulationExtender LazySimulationExtender;
        private ISimulationExtender LazyPredictionExtender;

        private IBuildingResearchProgressionExtender ProgressionExtender;

        public IScenarioSelectiveBuildingExtender SpecificScenarios(ScenarioSelector scenarioFilter)
        {
            ScenarioFilter = scenarioFilter;
            return this;
        }

        public IScenarioSelectiveBuildingExtender AllScenarios()
        {
            ScenarioFilter = AllScenariosFunc;
            return this;

            bool AllScenariosFunc(GameScenario scenario)
            {
                return true;
            }
        }

        public IDefinedBuildingExtender WithBuilding(IBuildingBuilder building, IBuildingGroupBuilder buildingGroup)
        {
            BuildingBuilder = building;
            BuildingGroupBuilder = buildingGroup;
            return this;
        }

        IDefinedPlaceableAccessibleBuildingExtender IDefinedPlaceableBuildingExtender.InToolbar(
            IToolbarEntryInsertLocation toolbarEntryInsertLocation)
        {
            ToolbarEntryInsertLocation = toolbarEntryInsertLocation;
            return this;
        }

        public IAtomicBuildingExtender WithSimulation<TSimulation>(
            IBuildingSimulationFactoryBuilder<TSimulation> buildingSimulationFactoryBuilder,
            ILogger logger)
            where TSimulation : ISimulation
        {
            LazySimulationExtender = new TypedSimulationExtender<TSimulation>(
                buildingSimulationFactoryBuilder: buildingSimulationFactoryBuilder,
                logger: logger);
            return this;
        }

        IDefinedPlaceableBuildingExtender IDefinedUnlockableBuildingExtender.WithDefaultPlacement()
        {
            return this;
        }

        public IAtomicBuildingExtender WithSimulation<TSimulation, TState, TConfig>(
            IBuildingSimulationFactoryBuilder<TSimulation, TState, TConfig> buildingSimulationFactoryBuilder,
            ILogger logger)
            where TSimulation : ISimulation
            where TState : class, ISimulationState, new()
        {
            LazySimulationExtender = new TypedSimulationExtender<TSimulation, TState, TConfig>(
                buildingSimulationFactoryBuilder: buildingSimulationFactoryBuilder,
                logger: logger);
            return this;
        }

        public IAtomicBuildingExtender
            WithSimulation<TSimulation, TConfig, TBaseConfiguration, TSimulationConfiguration>(
                IBuildingSimulationFactoryBuilder<TSimulation, TConfig, TBaseConfiguration>
                    buildingSimulationFactoryBuilder,
                ILogger logger)
        {
            throw new NotImplementedException();
        }

        IDefinedSimulatablePlaceableBuildingExtender IDefinedSimulatableBuildingExtender.WithDefaultPlacement()
        {
            return this;
        }

        IAtomicBuildingExtender IDefinedSimulatablePlaceableBuildingExtender.InToolbar(
            IToolbarEntryInsertLocation toolbarEntryInsertLocation)
        {
            ToolbarEntryInsertLocation = toolbarEntryInsertLocation;
            return this;
        }

        public IViewModularBuildingExtender WithAtomicShapeProcessingModules(
            ResearchSpeedId speedId,
            float processingDuration)
        {
            ModulesData = new BuildingModulesData(speedId: speedId, initialProcessingDuration: processingDuration);
            return this;
        }

        public IViewModularBuildingExtender WithCustomModules(IBuildingModules buildingModules)
        {
            ModulesData = new CustomBuildingsModulesData(buildingModules);
            return this;
        }

        public void Build()
        {
            BuildExtenders();

            // This method creates a tree of links for extending the game to add a building completely (base data,
            // simulation, placement, toolbar, modules).
            // Some of these extenders depend on data created on a previous step, but more importantly, they require
            // that the previous extension was applied to the game. Thus, this method create an extension logic tree
            // where the extenders are activated in the order they should. When all extenders are applied, the process
            // starts again (this happens, for example, when loading another savegame)
            void BuildExtenders()
            {
                // Start the chain of extenders with the scenario extender. This also serves as a filter for only
                // applying the other extenders if the scenario is part of the filter
                RewirerChainLink scenarioRewirer = RewirerChain.BeginRewiringWith(
                    new GameScenarioBuildingExtender(
                        scenarioFilter: ScenarioFilter,
                        progressionExtender: ProgressionExtender,
                        groupId: BuildingGroupBuilder.GroupId));

                // Then add the building group and building to the game buildings object
                var buildingRewirer = scenarioRewirer.ThenContinueRewiringWith(BuildBuildingExtender);

                // With the building added, create the simulation
                RewirerChainLink simulationsRewirer = LazySimulationExtender.ContinueAfter(buildingRewirer);

                RewirerChainLink predictionRewirer = LazyPredictionExtender?.ContinueAfter(buildingRewirer);

                // With the building, create the placement
                var placementRewirer = buildingRewirer.ThenContinueRewiringWith(BuildDefaultPlacementExtender);

                // And with the placement, create a toolbar entry
                var toolbarRewirer =
                    placementRewirer.ThenContinueRewiringWith(BuildToolbarExtender(ToolbarEntryInsertLocation));

                // And finally add the modules
                RewirerChainLink modulesRewirer = buildingRewirer.ThenContinueRewiringWith(BuildModulesExtender);

                // When all extenders are called (noticed that the specific order does not matter), the process is
                // restarted
                IWaitAllRewirers allRewirers = AggregatedChain.WaitFor(simulationsRewirer)
                                                              .And(toolbarRewirer)
                                                              .And(modulesRewirer);
                if (predictionRewirer != null)
                {
                    allRewirers = allRewirers.And(predictionRewirer);
                }

                allRewirers.AfterHijack.Register(OnApplyAllExtenders);
                return;

                void OnApplyAllExtenders()
                {
                    allRewirers.AfterHijack.Unregister(OnApplyAllExtenders);
                    BuildExtenders();
                }
            }
        }

        private BuildingsExtender BuildBuildingExtender()
        {
            return new BuildingsExtender(buildingBuilder: BuildingBuilder, buildingGroupBuilder: BuildingGroupBuilder);
        }

        private DefaultBuildingPlacementExtender BuildDefaultPlacementExtender(BuildingDefinition def)
        {
            return new DefaultBuildingPlacementExtender(def);
        }

        private Func<BuildingPlacementResult, ToolbarRewirer> BuildToolbarExtender(
            IToolbarEntryInsertLocation entryInsertLocation)
        {
            return BuildToolbarExtenderFunc;

            ToolbarRewirer BuildToolbarExtenderFunc(BuildingPlacementResult placementResult)
            {
                PlacementInitiatorId placement = placementResult.InitiatorId;
                var group = placementResult.Building.CustomData.Get<IBuildingDefinitionGroup>();

                return new ToolbarRewirer(
                    placement: placement,
                    title: group.Title,
                    description: group.Description,
                    icon: group.Icon,
                    entryInsertLocation: entryInsertLocation);
            }
        }

        private Func<BuildingDefinition, BuildingSimulationExtender<TSimulation, TState, TConfig>>
            BuildSimulationExtender<TSimulation, TState, TConfig>(
                IBuildingSimulationFactoryBuilder<TSimulation, TState, TConfig> buildingSimulationFactoryBuilder,
                ILogger logger)
            where TSimulation : ISimulation
            where TState : class, ISimulationState, new()
        {
            return BuildToolbarExtenderFunc;

            BuildingSimulationExtender<TSimulation, TState, TConfig> BuildToolbarExtenderFunc(
                BuildingDefinition buildingDefinition)
            {
                return new BuildingSimulationExtender<TSimulation, TState, TConfig>(
                    definitionId: buildingDefinition.Id,
                    buildingSimulationFactoryBuilder: buildingSimulationFactoryBuilder,
                    logger: logger);
            }
        }

        private IChainableRewirer BuildModulesExtender(BuildingDefinition buildingDefinition)
        {
            return new BuildingModulesExtender(buildingDefinition: buildingDefinition, data: ModulesData);
        }

        private class TypedSimulationExtender<TSimulation> : ISimulationExtender
            where TSimulation : ISimulation
        {
            private readonly IBuildingSimulationFactoryBuilder<TSimulation> BuildingSimulationFactoryBuilder;
            private readonly ILogger Logger;

            public TypedSimulationExtender(
                IBuildingSimulationFactoryBuilder<TSimulation> buildingSimulationFactoryBuilder,
                ILogger logger)
            {
                BuildingSimulationFactoryBuilder = buildingSimulationFactoryBuilder;
                Logger = logger;
            }

            public RewirerChainLink ContinueAfter(RewirerChainLink<BuildingDefinition> rewirerChainLink)
            {
                return rewirerChainLink.ThenContinueRewiringWith(BuildToolbarExtenderFunc);

                BuildingSimulationExtender<TSimulation> BuildToolbarExtenderFunc(BuildingDefinition buildingDefinition)
                {
                    return new BuildingSimulationExtender<TSimulation>(
                        definitionId: buildingDefinition.Id,
                        buildingSimulationFactoryBuilder: BuildingSimulationFactoryBuilder,
                        logger: Logger);
                }
            }
        }

        private class TypedSimulationExtender<TSimulation, TState, TConfig> : ISimulationExtender
            where TSimulation : ISimulation
            where TState : class, ISimulationState, new()

        {
            private readonly IBuildingSimulationFactoryBuilder<TSimulation, TState, TConfig>
                BuildingSimulationFactoryBuilder;

            private readonly ILogger Logger;

            public TypedSimulationExtender(
                IBuildingSimulationFactoryBuilder<TSimulation, TState, TConfig> buildingSimulationFactoryBuilder,
                ILogger logger)
            {
                BuildingSimulationFactoryBuilder = buildingSimulationFactoryBuilder;
                Logger = logger;
            }

            public RewirerChainLink ContinueAfter(RewirerChainLink<BuildingDefinition> rewirerChainLink)
            {
                return rewirerChainLink.ThenContinueRewiringWith(BuildToolbarExtenderFunc)
                                       .ThenContinueRewiringWith(BuildBuffablesExtender);

                BuildingSimulationExtender<TSimulation, TState, TConfig> BuildToolbarExtenderFunc(
                    BuildingDefinition buildingDefinition)
                {
                    return new BuildingSimulationExtender<TSimulation, TState, TConfig>(
                        definitionId: buildingDefinition.Id,
                        buildingSimulationFactoryBuilder: BuildingSimulationFactoryBuilder,
                        logger: Logger);
                }

                IChainableRewirer BuildBuffablesExtender(TConfig config)
                {
                    return new BuffablesExtender<TConfig>(config);
                }
            }
        }

        private class TypedPredictionExtender<TSimulation> : ISimulationExtender
            where TSimulation : IItemPredictionSimulation

        {
            private readonly IBuildingPredictionFactoryBuilder<TSimulation> BuildingSimulationFactoryBuilder;
            private readonly ILogger Logger;

            public TypedPredictionExtender(
                IBuildingPredictionFactoryBuilder<TSimulation> buildingSimulationFactoryBuilder,
                ILogger logger)
            {
                BuildingSimulationFactoryBuilder = buildingSimulationFactoryBuilder;
                Logger = logger;
            }

            public RewirerChainLink ContinueAfter(RewirerChainLink<BuildingDefinition> rewirerChainLink)
            {
                return rewirerChainLink.ThenContinueRewiringWith(BuildToolbarExtenderFunc);

                BuildingPredictionExtender<TSimulation> BuildToolbarExtenderFunc(BuildingDefinition buildingDefinition)
                {
                    return new BuildingPredictionExtender<TSimulation>(
                        definitionId: buildingDefinition.Id,
                        buildingSimulationFactoryBuilder: BuildingSimulationFactoryBuilder,
                        logger: Logger);
                }
            }
        }

        private class TypedPredictionExtender<TSimulation, TConfig> : ISimulationExtender
            where TSimulation : IItemPredictionSimulation

        {
            private readonly IBuildingPredictionFactoryBuilder<TSimulation, TConfig> BuildingSimulationFactoryBuilder;
            private readonly ILogger Logger;

            public TypedPredictionExtender(
                IBuildingPredictionFactoryBuilder<TSimulation, TConfig> buildingSimulationFactoryBuilder,
                ILogger logger)
            {
                BuildingSimulationFactoryBuilder = buildingSimulationFactoryBuilder;
                Logger = logger;
            }

            public RewirerChainLink ContinueAfter(RewirerChainLink<BuildingDefinition> rewirerChainLink)
            {
                return rewirerChainLink.ThenContinueRewiringWith(BuildToolbarExtenderFunc)
                                       .ThenContinueRewiringWith(BuildBuffablesExtender);

                BuildingPredictionExtender<TSimulation, TConfig> BuildToolbarExtenderFunc(
                    BuildingDefinition buildingDefinition)
                {
                    return new BuildingPredictionExtender<TSimulation, TConfig>(
                        definitionId: buildingDefinition.Id,
                        buildingSimulationFactoryBuilder: BuildingSimulationFactoryBuilder,
                        logger: Logger);
                }

                IChainableRewirer BuildBuffablesExtender(TConfig config)
                {
                    return new BuffablesExtender<TConfig>(config);
                }
            }
        }

        private interface ISimulationExtender
        {
            RewirerChainLink ContinueAfter(RewirerChainLink<BuildingDefinition> rewirerChainLink);
        }

        public IDefinedUnlockableBuildingExtender UnlockedAtMilestone(IMilestoneSelector milestoneSelector)
        {
            ProgressionExtender = new UnlockBuildingWithMilestoneResearchProgressionExtender(milestoneSelector);
            return this;
        }

        public IDefinedUnlockableBuildingExtender UnlockedWithNewSideUpgrade(
            IPresentableUnlockableSideUpgradeBuilder sideUpgradeBuilder)
        {
            ProgressionExtender = new UnlockBuildingWithNewSideUpgradeResearchProgressionExtender(sideUpgradeBuilder);
            return this;
        }

        public IDefinedUnlockableBuildingExtender UnlockedWithExistingSideUpgrade(
            ISideUpgradeSelector sideUpgradeSelector)
        {
            ProgressionExtender =
                new UnlockBuildingWithExistingSideUpgradeResearchProgressionExtender(sideUpgradeSelector);
            return this;
        }

        private enum ResearchProgression
        {
            Milestone,
            NewSideUpgrade,
            ExistingSideUpgrade
        }

        public IBuildingExtender WithPrediction<TPrediction>(
            IBuildingPredictionFactoryBuilder<TPrediction> predictionBuilder,
            ILogger logger)
            where TPrediction : IItemPredictionSimulation
        {
            LazyPredictionExtender = new TypedPredictionExtender<TPrediction>(
                buildingSimulationFactoryBuilder: predictionBuilder,
                logger: logger);
            return this;
        }

        public IBuildingExtender WithPrediction<TPrediction, TConfig>(
            IBuildingPredictionFactoryBuilder<TPrediction, TConfig> predictionBuilder,
            ILogger logger)
            where TPrediction : IItemPredictionSimulation
        {
            LazyPredictionExtender = new TypedPredictionExtender<TPrediction, TConfig>(
                buildingSimulationFactoryBuilder: predictionBuilder,
                logger: logger);
            return this;
        }

        public IBuildingExtender WithoutPrediction()
        {
            return this;
        }
    }

    public interface IBaseBuildingExtender
    {
        IScenarioSelectiveBuildingExtender SpecificScenarios(ScenarioSelector scenarioFilter);

        IScenarioSelectiveBuildingExtender AllScenarios();
    }

    // Scenario
    public interface IScenarioSelectiveBuildingExtender
    {
        IDefinedBuildingExtender WithBuilding(IBuildingBuilder building, IBuildingGroupBuilder buildingGroup);
    }

    // Scenario -> Building
    public interface IDefinedBuildingExtender
    {
        IDefinedUnlockableBuildingExtender UnlockedAtMilestone(IMilestoneSelector milestoneSelector);

        IDefinedUnlockableBuildingExtender UnlockedWithNewSideUpgrade(
            IPresentableUnlockableSideUpgradeBuilder sideUpgradeBuilder);

        IDefinedUnlockableBuildingExtender UnlockedWithExistingSideUpgrade(ISideUpgradeSelector sideUpgradeSelector);
    }

    // Scenario -> Building -> Research

    public interface IDefinedUnlockableBuildingExtender
    {
        IDefinedPlaceableBuildingExtender WithDefaultPlacement();

        IAtomicBuildingExtender WithSimulation<TSimulation, TState, TConfig>(
            IBuildingSimulationFactoryBuilder<TSimulation, TState, TConfig> buildingSimulationFactoryBuilder,
            ILogger logger)
            where TSimulation : ISimulation
            where TState : class, ISimulationState, new();

        IAtomicBuildingExtender WithSimulation<TSimulation, TState, TBaseConfiguration, TSimulationConfiguration>(
            IBuildingSimulationFactoryBuilder<TSimulation, TState, TBaseConfiguration> buildingSimulationFactoryBuilder,
            ILogger logger);
    }

    // Scenario -> Building -> Placement
    public interface IDefinedPlaceableBuildingExtender
    {
        IDefinedPlaceableAccessibleBuildingExtender InToolbar(IToolbarEntryInsertLocation entryInsertLocation);
    }

    // Scenario -> Building -> Placement -> Toolbar
    public interface IDefinedPlaceableAccessibleBuildingExtender
    {
        IAtomicBuildingExtender WithSimulation<TSimulation, TState, TConfig>(
            IBuildingSimulationFactoryBuilder<TSimulation, TState, TConfig> buildingSimulationFactoryBuilder,
            ILogger logger)
            where TSimulation : ISimulation
            where TState : class, ISimulationState, new();

        IAtomicBuildingExtender WithSimulation<TSimulation, TState, TBaseConfiguration, TSimulationConfiguration>(
            IBuildingSimulationFactoryBuilder<TSimulation, TState, TBaseConfiguration> buildingSimulationFactoryBuilder,
            ILogger logger);
    }

    // Scenario -> Building -> Simulation -> Buff
    public interface IDefinedSimulatableBuildingExtender
    {
        IDefinedSimulatablePlaceableBuildingExtender WithDefaultPlacement();
    }

    // Scenario -> Building -> Simulation -> Buff -> Placement
    public interface IDefinedSimulatablePlaceableBuildingExtender
    {
        IAtomicBuildingExtender InToolbar(IToolbarEntryInsertLocation entryInsertLocation);
    }

    public interface IAtomicBuildingExtender
    {
        IViewModularBuildingExtender WithAtomicShapeProcessingModules(
            ResearchSpeedId speedId,
            float processingDuration);

        IViewModularBuildingExtender WithCustomModules(IBuildingModules buildingModules);
    }

    public interface IViewModularBuildingExtender
    {
        IBuildingExtender WithPrediction<TPrediction>(
            IBuildingPredictionFactoryBuilder<TPrediction> predictionBuilder,
            ILogger logger)
            where TPrediction : IItemPredictionSimulation;

        IBuildingExtender WithPrediction<TPrediction, TConfig>(
            IBuildingPredictionFactoryBuilder<TPrediction, TConfig> predictionBuilder,
            ILogger logger)
            where TPrediction : IItemPredictionSimulation;

        IBuildingExtender WithoutPrediction();
    }

    public interface IBuildingExtender
    {
        void Build();
    }
}
