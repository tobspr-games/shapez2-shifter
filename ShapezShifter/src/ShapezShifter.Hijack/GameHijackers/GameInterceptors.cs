using System;
using Core.Logging;

namespace ShapezShifter.Hijack
{
    internal class GameInterceptors : IDisposable
    {
        private readonly ToolbarInterceptor ToolbarInterceptor;
        private readonly PlacementInitiatorsInterceptor PlacementInterceptor;
        private readonly BuildingsInterceptor BuildingsInterceptor;
        private readonly BuildingModulesInterceptor BuildingModulesInterceptor;
        private readonly SerializedGameScenarioInterceptor SerializedGameScenarioInterceptor;
        private readonly GameScenarioInterceptor GameScenarioInterceptor;
        private readonly SimulationSystemsInterceptor SimulationSystemsInterceptor;
        private readonly PredictionSystemsInterceptor PredictionSystemsInterceptor;
        private readonly BuffablesInterceptor BuffablesInterceptor;
        private readonly IslandsInterceptor IslandsInterceptor;
        private readonly IslandModulesInterceptor IslandModulesInterceptor;
        private readonly SaveDataInterceptor SaveDataInterceptor;
        private readonly ConsoleInterceptor ConsoleInterceptor;
        private readonly TickInterceptor TickInterceptor;

        public GameInterceptors(IRewirerProvider rewirerProvider, ILogger logger)
        {
            ToolbarInterceptor = new ToolbarInterceptor(rewirerProvider);
            PlacementInterceptor = new PlacementInitiatorsInterceptor(rewirerProvider);
            BuildingsInterceptor = new BuildingsInterceptor(rewirerProvider: rewirerProvider, logger: logger);
            BuildingModulesInterceptor = new BuildingModulesInterceptor(rewirerProvider);
            IslandsInterceptor = new IslandsInterceptor(rewirerProvider: rewirerProvider, logger: logger);
            IslandModulesInterceptor = new IslandModulesInterceptor(rewirerProvider);
            SerializedGameScenarioInterceptor = new SerializedGameScenarioInterceptor(rewirerProvider: rewirerProvider, logger: logger);
            GameScenarioInterceptor = new GameScenarioInterceptor(rewirerProvider: rewirerProvider, logger: logger);
            SimulationSystemsInterceptor = new SimulationSystemsInterceptor(rewirerProvider);
            PredictionSystemsInterceptor = new PredictionSystemsInterceptor(rewirerProvider);
            BuffablesInterceptor = new BuffablesInterceptor(rewirerProvider);
            SaveDataInterceptor = new SaveDataInterceptor(rewirerProvider: rewirerProvider, logger: logger);
            ConsoleInterceptor = new ConsoleInterceptor(rewirerProvider: rewirerProvider, logger: logger);
            TickInterceptor = new TickInterceptor(rewirerProvider);
        }

        public void Dispose()
        {
            SerializedGameScenarioInterceptor.Dispose();
            GameScenarioInterceptor.Dispose();
            BuildingModulesInterceptor.Dispose();
            BuildingsInterceptor.Dispose();
            PlacementInterceptor.Dispose();
            ToolbarInterceptor.Dispose();
            SimulationSystemsInterceptor.Dispose();
            PredictionSystemsInterceptor.Dispose();
            BuffablesInterceptor.Dispose();
            IslandsInterceptor.Dispose();
            IslandModulesInterceptor.Dispose();
            SaveDataInterceptor.Dispose();
            ConsoleInterceptor.Dispose();
            TickInterceptor.Dispose();
        }
    }
}
