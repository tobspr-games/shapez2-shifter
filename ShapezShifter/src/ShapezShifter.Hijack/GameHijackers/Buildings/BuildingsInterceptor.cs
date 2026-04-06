using System;
using Core.Logging;
using Game.Core.Rendering.MeshGeneration;
using Global.Core;
using MonoMod.RuntimeDetour;
using ShapezShifter.SharpDetour;

namespace ShapezShifter.Hijack
{
    internal class BuildingsInterceptor : IDisposable
    {
        private readonly IRewirerProvider RewirerProvider;
        private readonly ILogger Logger;
        private readonly Hook BuildingsFactoryFromMetadataHook;

        public BuildingsInterceptor(IRewirerProvider rewirerProvider, ILogger logger)
        {
            RewirerProvider = rewirerProvider;
            Logger = logger;
            BuildingsFactoryFromMetadataHook =
                DetourHelper
                   .CreateStaticPostfixHook<GameModeBuildingsFactory, MetaGameModeBuildings, IMeshCache,
                        VisualThemeBaseResources, GameBuildings>(
                        original: (meta, meshCache, resources) =>
                            GameModeBuildingsFactory.FromMetadata(meta, meshCache, resources),
                        postfix: Postfix);
        }

        private GameBuildings Postfix(
            MetaGameModeBuildings metaBuildings,
            IMeshCache meshCache,
            VisualThemeBaseResources theme,
            GameBuildings gameBuildings)
        {
            var buildingsRewirers = RewirerProvider.RewirersOfType<IBuildingsRewirer>();

            Logger.Info?.Log("Intercepting buildings creation");

            int buildingsCount = gameBuildings.All.Count;

            foreach (IBuildingsRewirer buildingsRewirer in buildingsRewirers)
            {
                gameBuildings = buildingsRewirer.ModifyGameBuildings(
                    metaBuildings: metaBuildings,
                    gameBuildings: gameBuildings,
                    meshCache: meshCache,
                    theme: theme);
            }

            Logger.Info?.Log($"New buildings: {gameBuildings.All.Count} + {buildingsCount}");

            return gameBuildings;
        }

        public void Dispose()
        {
            BuildingsFactoryFromMetadataHook.Dispose();
        }
    }
}
