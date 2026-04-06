#nullable enable
using System;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow
{
    /// <summary>
    /// Extension methods to make mod development easier
    /// </summary>
    public static class ModExtensions
    {
        public static RewirerHandle RunPeriodically(this IMod mod, Action<GameSessionOrchestrator, float> action)
        {
            return TickRewirer.Register(action);
        }
    }
}
