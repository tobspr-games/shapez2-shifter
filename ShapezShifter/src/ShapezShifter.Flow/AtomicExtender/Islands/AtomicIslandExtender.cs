using System;
using Core.Logging;
using Game.Core.Simulation;
using ShapezShifter.Flow.Research;
using ShapezShifter.Flow.Toolbar;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    public class AtomicIslandExtender
        : IBaseIslandExtender,
          IScenarioSelectiveIslandExtender,
          IDefinedIslandExtender,
          IDefinedPlaceableIslandExtender,
          IDefinedPlaceableAccessibleIslandExtender,
          IDefinedSimulatableIslandExtender,
          IDefinedSimulatablePlaceableIslandExtender,
          IAtomicIslandExtender,
          IIslandExtender,
          IDefinedUnlockableIslandExtender,
          IDefinedAccessibleSimulatablePlaceableIslandExtender
    {
        // If each interface would have a specialized implementor, these fields could become
        // read-only. However that would create a lot of extra boiler-plate code

        private ScenarioSelector ScenarioFilter;

        private IToolbarEntryInsertLocation ToolbarEntryInsertLocation;
        private IIslandModulesData ModulesData;
        private IIslandBuilder IslandBuilder;
        private IIslandGroupBuilder IslandGroupBuilder;
        private ISimulationExtender LazySimulationExtender;
        private ISimulationExtender LazyPredictionExtender;
        private IIslandResearchProgressionExtender ProgressionExtender;

        public IScenarioSelectiveIslandExtender SpecificScenarios(ScenarioSelector scenarioFilter)
        {
            ScenarioFilter = scenarioFilter;
            return this;
        }

        public IScenarioSelectiveIslandExtender AllScenarios()
        {
            ScenarioFilter = AllScenariosFunc;
            return this;

            bool AllScenariosFunc(GameScenario scenario)
            {
                return true;
            }
        }

        public IDefinedIslandExtender WithIsland(IIslandBuilder island, IIslandGroupBuilder islandGroup)
        {
            IslandBuilder = island;
            IslandGroupBuilder = islandGroup;
            return this;
        }

        IDefinedPlaceableIslandExtender IDefinedUnlockableIslandExtender.WithDefaultPlacement()
        {
            return this;
        }

        IDefinedPlaceableAccessibleIslandExtender IDefinedPlaceableIslandExtender.InToolbar(
            IToolbarEntryInsertLocation toolbarEntryInsertLocation)
        {
            ToolbarEntryInsertLocation = toolbarEntryInsertLocation;
            return this;
        }

        public IAtomicIslandExtender WithSimulation<TSimulation>(
            IBuildingSimulationFactoryBuilder<TSimulation> buildingSimulationFactoryBuilder)
            where TSimulation : ISimulation
        {
            LazySimulationExtender = new TypedSimulationExtender<TSimulation>(buildingSimulationFactoryBuilder);
            return this;
        }

        public IAtomicIslandExtender WithSimulation<TSimulation, TState, TConfig>(
            IIslandSimulationFactoryBuilder<TSimulation, TState, TConfig> simulationFactoryBuilder)
            where TSimulation : Simulation<TState>
            where TState : class, ISimulationState, new()
        {
            LazySimulationExtender =
                new TypedSimulationExtender<TSimulation, TState, TConfig>(simulationFactoryBuilder);
            return this;
        }

        IAtomicIslandExtender IDefinedPlaceableAccessibleIslandExtender.WithoutSimulation()
        {
            return this;
        }

        IAtomicIslandExtender IDefinedUnlockableIslandExtender.WithoutSimulation()
        {
            return this;
        }

        public IAtomicIslandExtender WithSimulation<TSimulation, TConfig, TBaseConfiguration, TSimulationConfiguration>(
            IIslandSimulationFactoryBuilder<TSimulation, TConfig, TBaseConfiguration> simulationFactoryBuilder)
        {
            throw new NotImplementedException();
        }

        IDefinedSimulatablePlaceableIslandExtender IDefinedSimulatableIslandExtender.WithDefaultPlacement()
        {
            return this;
        }

        IDefinedAccessibleSimulatablePlaceableIslandExtender IDefinedSimulatablePlaceableIslandExtender.InToolbar(
            IToolbarEntryInsertLocation toolbarEntryInsertLocation)
        {
            ToolbarEntryInsertLocation = toolbarEntryInsertLocation;
            return this;
        }

        public IIslandExtender WithCustomModules(IIslandModuleDataProvider islandModules)
        {
            ModulesData = new CustomIslandsModulesData(islandModules);
            return this;
        }

        public IIslandExtender WithoutModules()
        {
            ModulesData = new NoModulesData();
            return this;
        }

        public void Build()
        {
            RewirerHandle handle = GameRewirers.ReserveHandle();
            BuildExtenders();

            // This method creates a tree of links for extending the game to add a island completely (base data,
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
                    handle,
                    new GameScenarioIslandExtender(
                        scenarioFilter: ScenarioFilter,
                        progressionExtender: ProgressionExtender,
                        groupId: IslandGroupBuilder.GroupId));

                // Then add the island group and island to the game islands object
                var islandRewirer = scenarioRewirer.ThenContinueRewiringWith(BuildIslandExtender);

                RewirerChainLink simulationsRewirer = null;
                if (LazySimulationExtender != null)
                {
                    // With the island added, create the simulation
                    simulationsRewirer = LazySimulationExtender.ContinueAfter(islandRewirer);
                }

                RewirerChainLink predictionsRewirer = null;
                if (LazyPredictionExtender != null)
                {
                    predictionsRewirer = LazyPredictionExtender.ContinueAfter(islandRewirer);
                }

                // With the island, create the placement
                var placementRewirer = islandRewirer.ThenContinueRewiringWith(BuildDefaultPlacementExtender);

                // And with the placement, create a toolbar entry
                var toolbarRewirer =
                    placementRewirer.ThenContinueRewiringWith(BuildToolbarExtender(ToolbarEntryInsertLocation));

                // And finally add the modules
                RewirerChainLink modulesRewirer = islandRewirer.ThenContinueRewiringWith(BuildModulesExtender);

                // When all extenders are called (noticed that the specific order does not matter), the process is
                // restarted
                IWaitAllRewirers modulesAndToolbar = AggregatedChain.WaitFor(modulesRewirer).And(toolbarRewirer);

                IWaitAllRewirers allRewirersToWait = modulesAndToolbar;
                if (simulationsRewirer != null)
                {
                    allRewirersToWait = allRewirersToWait.And(simulationsRewirer);
                }

                if (predictionsRewirer != null)
                {
                    allRewirersToWait = allRewirersToWait.And(predictionsRewirer);
                }

                allRewirersToWait.AfterHijack.Register(OnApplyAllExtenders);
                return;

                void OnApplyAllExtenders()
                {
                    allRewirersToWait.AfterHijack.Unregister(OnApplyAllExtenders);
                    BuildExtenders();
                }
            }
        }

        private IslandsExtender BuildIslandExtender()
        {
            return new IslandsExtender(islandBuilder: IslandBuilder, islandGroupBuilder: IslandGroupBuilder);
        }

        private DefaultIslandPlacementExtender BuildDefaultPlacementExtender(IslandDefinition def)
        {
            return new DefaultIslandPlacementExtender(def);
        }

        private Func<IslandPlacementResult, ToolbarRewirer> BuildToolbarExtender(
            IToolbarEntryInsertLocation entryInsertLocation)
        {
            return BuildToolbarExtenderFunc;

            ToolbarRewirer BuildToolbarExtenderFunc(IslandPlacementResult placementResult)
            {
                PlacementInitiatorId placement = placementResult.InitiatorId;
                var group = placementResult.Island.CustomData.Get<IIslandDefinitionGroup>();

                var presentation = group.CustomData.Get<GroupPresentationData>();

                return new ToolbarRewirer(
                    placement: placement,
                    title: presentation.Title,
                    description: presentation.Description,
                    icon: presentation.Icon,
                    entryInsertLocation: entryInsertLocation);
            }
        }

        private Func<IslandDefinition, IslandSimulationExtender<TSimulation, TState, TConfig>>
            BuildSimulationExtender<TSimulation, TState, TConfig>(
                IIslandSimulationFactoryBuilder<TSimulation, TState, TConfig> simulationFactoryBuilder)
            where TSimulation : Simulation<TState>
            where TState : class, ISimulationState, new()
        {
            return BuildToolbarExtenderFunc;

            IslandSimulationExtender<TSimulation, TState, TConfig> BuildToolbarExtenderFunc(
                IslandDefinition islandDefinition)
            {
                return new IslandSimulationExtender<TSimulation, TState, TConfig>(
                    definitionId: islandDefinition.Id,
                    simulationFactoryBuilder: simulationFactoryBuilder);
            }
        }

        private IChainableRewirer BuildModulesExtender(IslandDefinition islandDefinition)
        {
            return new IslandModulesExtender(buildingDefinition: islandDefinition, data: ModulesData);
        }

        private class TypedSimulationExtender<TSimulation> : ISimulationExtender
            where TSimulation : ISimulation

        {
            private readonly IBuildingSimulationFactoryBuilder<TSimulation> BuildingSimulationFactoryBuilder;

            public TypedSimulationExtender(
                IBuildingSimulationFactoryBuilder<TSimulation> buildingSimulationFactoryBuilder)
            {
                BuildingSimulationFactoryBuilder = buildingSimulationFactoryBuilder;
            }

            public RewirerChainLink ContinueAfter(RewirerChainLink<IslandDefinition> rewirerChainLink)
            {
                return rewirerChainLink.ThenContinueRewiringWith(BuildToolbarExtenderFunc);

                IslandSimulationExtender<TSimulation> BuildToolbarExtenderFunc(IslandDefinition islandDefinition)
                {
                    return new IslandSimulationExtender<TSimulation>(
                        definitionId: islandDefinition.Id,
                        buildingSimulationFactoryBuilder: BuildingSimulationFactoryBuilder);
                }
            }
        }

        private class TypedSimulationExtender<TSimulation, TState, TConfig> : ISimulationExtender
            where TSimulation : Simulation<TState>
            where TState : class, ISimulationState, new()

        {
            private readonly IIslandSimulationFactoryBuilder<TSimulation, TState, TConfig> SimulationFactoryBuilder;

            public TypedSimulationExtender(
                IIslandSimulationFactoryBuilder<TSimulation, TState, TConfig> simulationFactoryBuilder)
            {
                SimulationFactoryBuilder = simulationFactoryBuilder;
            }

            public RewirerChainLink ContinueAfter(RewirerChainLink<IslandDefinition> rewirerChainLink)
            {
                return rewirerChainLink.ThenContinueRewiringWith(BuildToolbarExtenderFunc)
                                       .ThenContinueRewiringWith(BuildBuffablesExtender);

                IslandSimulationExtender<TSimulation, TState, TConfig> BuildToolbarExtenderFunc(
                    IslandDefinition islandDefinition)
                {
                    return new IslandSimulationExtender<TSimulation, TState, TConfig>(
                        definitionId: islandDefinition.Id,
                        simulationFactoryBuilder: SimulationFactoryBuilder);
                }

                IChainableRewirer BuildBuffablesExtender(TConfig config)
                {
                    return new BuffablesExtender<TConfig>(config);
                }
            }
        }

        private class TypedPredictionExtender<TSimulation> : ISimulationExtender
            where TSimulation : ISimulation

        {
            private readonly IIslandPredictionFactoryBuilder<TSimulation> IslandSimulationFactoryBuilder;
            private readonly ILogger Logger;

            public TypedPredictionExtender(
                IIslandPredictionFactoryBuilder<TSimulation> islandSimulationFactoryBuilder,
                ILogger logger)
            {
                IslandSimulationFactoryBuilder = islandSimulationFactoryBuilder;
                Logger = logger;
            }

            public RewirerChainLink ContinueAfter(RewirerChainLink<IslandDefinition> rewirerChainLink)
            {
                return rewirerChainLink.ThenContinueRewiringWith(BuildToolbarExtenderFunc);

                IslandPredictionExtender<TSimulation> BuildToolbarExtenderFunc(IslandDefinition islandDefinition)
                {
                    return new IslandPredictionExtender<TSimulation>(
                        definitionId: islandDefinition.Id,
                        islandSimulationFactoryBuilder: IslandSimulationFactoryBuilder,
                        logger: Logger);
                }
            }
        }

        private class TypedPredictionExtender<TSimulation, TConfig> : ISimulationExtender
            where TSimulation : ISimulation

        {
            private readonly IIslandPredictionFactoryBuilder<TSimulation, TConfig> IslandSimulationFactoryBuilder;
            private readonly ILogger Logger;

            public TypedPredictionExtender(
                IIslandPredictionFactoryBuilder<TSimulation, TConfig> islandSimulationFactoryBuilder,
                ILogger logger)
            {
                IslandSimulationFactoryBuilder = islandSimulationFactoryBuilder;
                Logger = logger;
            }

            public RewirerChainLink ContinueAfter(RewirerChainLink<IslandDefinition> rewirerChainLink)
            {
                return rewirerChainLink.ThenContinueRewiringWith(BuildToolbarExtenderFunc)
                                       .ThenContinueRewiringWith(BuildBuffablesExtender);

                IslandPredictionExtender<TSimulation, TConfig> BuildToolbarExtenderFunc(
                    IslandDefinition islandDefinition)
                {
                    return new IslandPredictionExtender<TSimulation, TConfig>(
                        definitionId: islandDefinition.Id,
                        buildingSimulationFactoryBuilder: IslandSimulationFactoryBuilder,
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
            RewirerChainLink ContinueAfter(RewirerChainLink<IslandDefinition> rewirerChainLink);
        }

        public IDefinedUnlockableIslandExtender UnlockedAtMilestone(IMilestoneSelector milestoneSelector)
        {
            ProgressionExtender = new UnlockIslandWithMilestoneResearchProgressionExtender(milestoneSelector);
            return this;
        }

        public IDefinedUnlockableIslandExtender UnlockedWithNewSideUpgrade(
            IPresentableUnlockableSideUpgradeBuilder sideUpgradeBuilder)
        {
            ProgressionExtender = new UnlockIslandWithNewSideUpgradeResearchProgressionExtender(sideUpgradeBuilder);
            return this;
        }

        public IDefinedUnlockableIslandExtender UnlockedWithExistingSideUpgrade(
            ISideUpgradeSelector sideUpgradeSelector)
        {
            ProgressionExtender =
                new UnlockIslandWithExistingSideUpgradeResearchProgressionExtender(sideUpgradeSelector);
            return this;
        }

        public IAtomicIslandExtender WithPrediction<TPrediction>(
            IIslandPredictionFactoryBuilder<TPrediction> predictionBuilder,
            ILogger logger)
            where TPrediction : ISimulation
        {
            LazyPredictionExtender = new TypedPredictionExtender<TPrediction>(
                islandSimulationFactoryBuilder: predictionBuilder,
                logger: logger);
            return this;
        }

        public IAtomicIslandExtender WithPrediction<TPrediction, TConfig>(
            IIslandPredictionFactoryBuilder<TPrediction, TConfig> predictionBuilder,
            ILogger logger)
            where TPrediction : ISimulation
        {
            LazyPredictionExtender = new TypedPredictionExtender<TPrediction, TConfig>(
                islandSimulationFactoryBuilder: predictionBuilder,
                logger: logger);
            return this;
        }

        public IAtomicIslandExtender WithoutPrediction(IToolbarEntryInsertLocation entryInsertLocation)
        {
            throw new NotImplementedException();
        }
    }

    public interface IBaseIslandExtender
    {
        IScenarioSelectiveIslandExtender SpecificScenarios(ScenarioSelector scenarioFilter);

        IScenarioSelectiveIslandExtender AllScenarios();
    }

    // Scenario
    public interface IScenarioSelectiveIslandExtender
    {
        IDefinedIslandExtender WithIsland(IIslandBuilder island, IIslandGroupBuilder islandGroup);
    }

    // Scenario -> Island
    public interface IDefinedIslandExtender
    {
        IDefinedUnlockableIslandExtender UnlockedAtMilestone(IMilestoneSelector milestoneSelector);

        IDefinedUnlockableIslandExtender UnlockedWithNewSideUpgrade(
            IPresentableUnlockableSideUpgradeBuilder sideUpgradeBuilder);

        IDefinedUnlockableIslandExtender UnlockedWithExistingSideUpgrade(ISideUpgradeSelector sideUpgradeSelector);
    }

    public interface IDefinedUnlockableIslandExtender
    {
        IDefinedPlaceableIslandExtender WithDefaultPlacement();

        IAtomicIslandExtender WithSimulation<TSimulation>(
            IBuildingSimulationFactoryBuilder<TSimulation> buildingSimulationFactoryBuilder)
            where TSimulation : ISimulation;

        IAtomicIslandExtender WithSimulation<TSimulation, TState, TConfig>(
            IIslandSimulationFactoryBuilder<TSimulation, TState, TConfig> simulationFactoryBuilder)
            where TSimulation : Simulation<TState>
            where TState : class, ISimulationState, new();

        IAtomicIslandExtender WithoutSimulation();

        IAtomicIslandExtender WithSimulation<TSimulation, TState, TBaseConfiguration, TSimulationConfiguration>(
            IIslandSimulationFactoryBuilder<TSimulation, TState, TBaseConfiguration> simulationFactoryBuilder);
    }

    // Scenario -> Island -> Placement
    public interface IDefinedPlaceableIslandExtender
    {
        IDefinedPlaceableAccessibleIslandExtender InToolbar(IToolbarEntryInsertLocation entryInsertLocation);
    }

    // Scenario -> Island -> Placement -> Toolbar
    public interface IDefinedPlaceableAccessibleIslandExtender
    {
        IAtomicIslandExtender WithSimulation<TSimulation>(
            IBuildingSimulationFactoryBuilder<TSimulation> buildingSimulationFactoryBuilder)
            where TSimulation : ISimulation;

        IAtomicIslandExtender WithSimulation<TSimulation, TState, TConfig>(
            IIslandSimulationFactoryBuilder<TSimulation, TState, TConfig> simulationFactoryBuilder)
            where TSimulation : Simulation<TState>
            where TState : class, ISimulationState, new();

        IAtomicIslandExtender WithoutSimulation();

        IAtomicIslandExtender WithSimulation<TSimulation, TState, TBaseConfiguration, TSimulationConfiguration>(
            IIslandSimulationFactoryBuilder<TSimulation, TState, TBaseConfiguration> simulationFactoryBuilder);
    }

    // Scenario -> Island -> Simulation -> Buff
    public interface IDefinedSimulatableIslandExtender
    {
        IDefinedSimulatablePlaceableIslandExtender WithDefaultPlacement();
    }

    // Scenario -> Island -> Simulation -> Buff -> Placement
    public interface IDefinedSimulatablePlaceableIslandExtender
    {
        IDefinedAccessibleSimulatablePlaceableIslandExtender InToolbar(IToolbarEntryInsertLocation entryInsertLocation);
    }

    public interface IDefinedAccessibleSimulatablePlaceableIslandExtender
    {
        public IAtomicIslandExtender WithPrediction<TPrediction>(
            IIslandPredictionFactoryBuilder<TPrediction> predictionBuilder,
            ILogger logger)
            where TPrediction : ISimulation;

        public IAtomicIslandExtender WithPrediction<TPrediction, TConfig>(
            IIslandPredictionFactoryBuilder<TPrediction, TConfig> predictionBuilder,
            ILogger logger)
            where TPrediction : ISimulation;

        IAtomicIslandExtender WithoutPrediction(IToolbarEntryInsertLocation entryInsertLocation);
    }

    public interface IAtomicIslandExtender
    {
        IIslandExtender WithCustomModules(IIslandModuleDataProvider islandModules);

        IIslandExtender WithoutModules();
    }

    public interface IIslandExtender
    {
        void Build();
    }
}
