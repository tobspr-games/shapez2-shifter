using System;
using System.Collections.Generic;
using Core.Logging;
using Game.Core.Content.Buildings;
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
                   .CreateStaticPostfixHook<IBuildingCatalogPair, AuthoringBuildings, IMeshCache,
                        VisualThemeBaseResources, GameBuildings>(
                        original: (catalog, meta, meshCache, resources) =>
                            GameModeBuildingsFactory.FromMetadata(catalog, meta, meshCache, resources),
                        postfix: Postfix);
        }

        public void Dispose()
        {
            BuildingsFactoryFromMetadataHook.Dispose();
        }

        private GameBuildings Postfix(
            IBuildingCatalogPair catalog,
            AuthoringBuildings meta,
            IMeshCache meshCache,
            VisualThemeBaseResources theme,
            GameBuildings gameBuildings)
        {
            IEnumerable<IBuildingsRewirer> buildingsRewirers = RewirerProvider.RewirersOfType<IBuildingsRewirer>();

            Logger.Info?.Log("Intercepting buildings creation");

            int buildingsCount = gameBuildings.All.Count;

            foreach (IBuildingsRewirer buildingsRewirer in buildingsRewirers)
            {
                gameBuildings = buildingsRewirer.ModifyGameBuildings(
                    meta,
                    gameBuildings: gameBuildings,
                    meshCache: meshCache,
                    theme: theme);
            }

            Logger.Info?.Log($"New buildings: {gameBuildings.All.Count} + {buildingsCount}");

            return gameBuildings;
        }
    }
}
