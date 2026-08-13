using System;
using Core.Logging;
using Game.Core.Content.Islands;
using MonoMod.RuntimeDetour;
using ShapezShifter.SharpDetour;

namespace ShapezShifter.Hijack
{
    internal class IslandsInterceptor : IDisposable
    {
        private readonly IRewirerProvider RewirerProvider;
        private readonly ILogger Logger;
        private readonly Hook IslandsFactoryFromMetadataHook;

        public IslandsInterceptor(IRewirerProvider rewirerProvider, ILogger logger)
        {
            RewirerProvider = rewirerProvider;
            Logger = logger;
            IslandsFactoryFromMetadataHook =
                DetourHelper.CreatePostfixHook<IslandDefinitionFactory, IIslandCatalogPair, AuthoringIslands, GameIslands>(
                    original: (factory, pair, meta) => factory.BakeMetadataIntoRuntime(pair, meta),
                    postfix: Postfix);
        }

        private GameIslands Postfix(
            IslandDefinitionFactory islandDefinitionFactory,
            IIslandCatalogPair catalogPair,
            AuthoringIslands metaIslands,
            GameIslands gameIslands)
        {
            var islandsRewirers = RewirerProvider.RewirersOfType<IIslandsRewirer>();

            Logger.Info?.Log("Intercepting islands creation");

            int islandsCount = gameIslands.AllDefinitions.Count;

            foreach (IIslandsRewirer islandsRewirer in islandsRewirers)
            {
                gameIslands = islandsRewirer.ModifyGameIslands(
                    factory: islandDefinitionFactory,
                    metaIslands: metaIslands,
                    gameIslands: gameIslands);
            }

            Logger.Info?.Log($"New islands: {gameIslands.AllDefinitions.Count} + {islandsCount}");

            return gameIslands;
        }

        public void Dispose()
        {
            IslandsFactoryFromMetadataHook.Dispose();
        }
    }
}
