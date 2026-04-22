#nullable enable
using System;
using System.Collections.Generic;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow
{
    public static class ModConsoleCommandsCreator
    {
        public static ModConsoleRewirer AddModCommands(this IMod mod)
        {
            return new ModConsoleRewirer(mod);
        }

        public class ModConsoleRewirer : IConsoleRewirer, IDisposable
        {
            private readonly IMod Mod;
            private readonly List<Action<IDebugConsole>> ConsoleCommandsAddActions = new();
            private readonly RewirerHandle Handle;

            public ModConsoleRewirer(IMod mod)
            {
                Mod = mod;
                Handle = GameRewirers.AddRewirer(this);
            }

            public void AddCommand(Action<IDebugConsole> command)
            {
                ConsoleCommandsAddActions.Add(command);
            }

            public void RegisterCommands(IDebugConsole console)
            {
                var wrapped = new ModConsoleCommandWrapper(mod: Mod, console: console);
                foreach (var consoleCommand in ConsoleCommandsAddActions)
                {
                    consoleCommand.Invoke(wrapped);
                }
            }

            public void Dispose()
            {
                GameRewirers.RemoveRewirer(Handle);
            }
        }

        private class ModConsoleCommandWrapper : IDebugConsole
        {
            private readonly IDebugConsole Console;
            private readonly string ModPrefix;

            public ModConsoleCommandWrapper(IMod mod, IDebugConsole console)
            {
                Console = console;
                ModPrefix = mod.GetType().Assembly.GetName().Name;
            }

            public List<string> GetAutoCompletions(string start)
            {
                return Console.GetAutoCompletions(start);
            }

            public void ParseAndExecute(string command, Action<string> output)
            {
                Console.ParseAndExecute(command: command, output: output);
            }

            public void Register(string id, Action<DebugConsole.CommandContext> handler, bool isCheat = false)
            {
                Console.Register(id: $"{ModPrefix}.{id}", handler: handler, isCheat: isCheat);
            }

            public void Register(
                string id,
                DebugConsole.ConsoleOption option0,
                Action<DebugConsole.CommandContext> handler,
                bool isCheat = false)
            {
                Console.Register(id: $"{ModPrefix}.{id}", handler: handler, isCheat: isCheat, option0: option0);
            }

            public void Register(
                string id,
                DebugConsole.ConsoleOption option0,
                DebugConsole.ConsoleOption option1,
                Action<DebugConsole.CommandContext> handler,
                bool isCheat = false)
            {
                Console.Register(
                    id: $"{ModPrefix}.{id}",
                    handler: handler,
                    isCheat: isCheat,
                    option0: option0,
                    option1: option1);
            }
        }
    }
}
