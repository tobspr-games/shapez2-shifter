#nullable enable
using System;
using Core.Logging;
using MonoMod.RuntimeDetour;
using ShapezShifter.SharpDetour;

namespace ShapezShifter.Hijack
{
    internal class ConsoleInterceptor : IDisposable
    {
        private readonly IRewirerProvider RewirerProvider;
        private readonly ILogger Logger;
        private readonly Hook ConsoleCommandsHook;

        public ConsoleInterceptor(IRewirerProvider rewirerProvider, ILogger logger)
        {
            RewirerProvider = rewirerProvider;
            Logger = logger;
            ConsoleCommandsHook = DetourHelper.CreatePostfixHook<GameSessionOrchestrator, IGameData, GlobalsData>(
                original: (orchestrator, gameData, globals) => orchestrator.Init_9_ConsoleCommands(gameData, globals),
                postfix: SetupConsoleCommands);
        }

        private void SetupConsoleCommands(GameSessionOrchestrator orchestrator, IGameData gameData, GlobalsData globals)
        {
            var console = orchestrator.DependencyContainer.Resolve<IDebugConsole>();

            var consoleRewirers = RewirerProvider.RewirersOfType<IConsoleRewirer>();

            foreach (IConsoleRewirer? consoleRewirer in consoleRewirers)
            {
                try
                {
                    consoleRewirer.RegisterCommands(console);
                }
                catch (Exception ex)
                {
                    Logger.Error?.Log(
                        $"Failed to register console commands from rewirer {consoleRewirer.GetType().Name}: {ex}");
                }
            }

            return;

            void RegisterCommand(
                string command,
                Action<DebugConsole.CommandContext> handler,
                bool isCheat,
                DebugConsole.ConsoleOption? arg1,
                DebugConsole.ConsoleOption? arg2)
            {
                Logger.Info?.Log($"Registering console command: {command}");
                if (arg1 != null && arg2 != null)
                {
                    console.Register(id: command, option0: arg1, option1: arg2, handler: handler, isCheat: isCheat);
                }
                else if (arg1 != null && arg2 == null)
                {
                    console.Register(id: command, option0: arg1, handler: handler, isCheat: isCheat);
                }
                else if (arg1 == null && arg2 != null)
                {
                    console.Register(id: command, option0: arg2, handler: handler, isCheat: isCheat);
                }
                else
                {
                    console.Register(id: command, handler: handler, isCheat: isCheat);
                }
            }
        }

        public void Dispose()
        {
            ConsoleCommandsHook.Dispose();
        }
    }
}
