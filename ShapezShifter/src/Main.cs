using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using ShapezShifter.Hijack;
using UnityEngine;
using ILogger = Core.Logging.ILogger;

[assembly: InternalsVisibleTo("ShapezShifterTests")]

namespace ShapezShifter
{
    [UsedImplicitly]
    public class Main : IMod
    {
        private readonly ILogger Logger;

        private readonly GameInterceptors GameInterceptors;
        private bool Disposed;

        public Main(ILogger logger)
        {
            logger.Info?.Log("Shapez Shifter Initialized");
            Logger = logger;
            Debugging.Logger = logger;

            SetupPathEnvironmentVariable(logger);

            CachedStaticallyAccessibleRewirerProvider staticallyAccessibleRewirerProvider = new(logger);
            GameInterceptors = new GameInterceptors(
                rewirerProvider: staticallyAccessibleRewirerProvider,
                logger: logger);
        }

        private static void SetupPathEnvironmentVariable(ILogger logger)
        {
            if (Application.isEditor || !GameEnvironment.ShouldSetModFriendlyEnvironmentPath)
            {
                return;
            }

            const string environmentVariable = "SPZ2_SHIFTER";
            string path = typeof(Main).Assembly.Location;
            logger.Info?.Log($"Setting environment variable {environmentVariable} to {path}");
            Environment.SetEnvironmentVariable(
                variable: environmentVariable,
                value: path,
                target: EnvironmentVariableTarget.User);
        }

        public void Dispose()
        {
            if (Disposed)
            {
                Logger.Warning?.Log("Mod was already disposed");
                return;
            }

            Disposed = true;
            GameInterceptors.Dispose();
        }
    }

    internal static class Debugging
    {
        public static ILogger Logger;
    }
}
