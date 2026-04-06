#nullable enable
using System;
using System.Linq;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow
{
    /// <summary>
    /// Extension methods to make mod development easier
    /// </summary>
    public static class ModExtensions
    {
        /// <summary>
        /// Register a console command for this mod
        /// </summary>
        /// <param name="mod">The mod instance</param>
        /// <param name="commandName">The command name (e.g., "hello"). Must be lowercase</param>
        /// <param name="handler">The command handler</param>
        /// <param name="isCheat">Whether the command needs cheats enabled to run or not</param>
        /// <param name="arg1">The first argument of the command (optional)</param>
        /// <param name="arg2">The second argument of the command (optional)</param>
        /// <param name="useAssemblyPrefix">Whether to prefix the command with the assembly name</param>
        /// <returns>A rewirer handle that can be used to unregister the command</returns>
        public static RewirerHandle RegisterConsoleCommand(
            this IMod mod,
            string commandName,
            Action<DebugConsole.CommandContext> handler,
            bool isCheat = false,
            DebugConsole.ConsoleOption? arg1 = null,
            DebugConsole.ConsoleOption? arg2 = null,
            bool useAssemblyPrefix = true)
        {
            string? processedName = commandName;
            if (useAssemblyPrefix)
            {
                processedName = mod.GetType().Assembly.GetName().Name + "." + commandName;
            }
            if (processedName.Any(char.IsUpper))
            {
                Debugging.Logger?.Warning?.Log(
                    "Console commands can't contain uppercase characters. The command has been lowercased");
                processedName = processedName.ToLower();
            }
            return ConsoleCommand.Register(
                commandName: processedName,
                handler: handler,
                isCheat: isCheat,
                arg1: arg1,
                arg2: arg2);
        }

        public static RewirerHandle RunPeriodically(this IMod mod, Action<GameSessionOrchestrator, float> action)
        {
            return TickRewirer.Register(action);
        }
    }
}
