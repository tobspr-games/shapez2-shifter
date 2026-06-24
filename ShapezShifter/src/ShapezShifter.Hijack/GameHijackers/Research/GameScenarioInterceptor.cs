using System;
using System.Collections.Generic;
using System.Reflection;
using Core.Logging;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;

namespace ShapezShifter.Hijack
{
    internal class GameScenarioInterceptor : IDisposable
    {
        private readonly IRewirerProvider RewirerProvider;
        private readonly ILogger Logger;
        private readonly ILHook IlHook;

        public GameScenarioInterceptor(IRewirerProvider rewirerProvider, ILogger logger)
        {
            RewirerProvider = rewirerProvider;
            Logger = logger;

            MethodInfo target = typeof(GameMode).GetMethod("From", BindingFlags.Static | BindingFlags.Public);
            IlHook = new ILHook(
                target!,
                ctx =>
                {
                    var cursor = new ILCursor(ctx);
                    cursor.GotoNext(MoveType.After, i => i.MatchNewobj<GameScenario>());
                    cursor.EmitDelegate<Func<GameScenario, GameScenario>>(Postfix);
                });
        }

        public void Dispose()
        {
            IlHook.Dispose();
        }

        private GameScenario Postfix(GameScenario gameScenario)
        {
            Logger.Info?.Log("Modifying research");
            IEnumerable<IGameScenarioRewirer> scenarioRewirers = RewirerProvider.RewirersOfType<IGameScenarioRewirer>();
            foreach (IGameScenarioRewirer scenarioRewirer in scenarioRewirers)
            {
                gameScenario = scenarioRewirer.ModifyGameScenario(gameScenario);
            }

            return gameScenario;
        }
    }
}
