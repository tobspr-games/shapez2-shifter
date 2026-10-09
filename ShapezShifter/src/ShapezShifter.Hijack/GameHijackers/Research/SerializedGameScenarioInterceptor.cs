using System;
using System.Collections.Generic;
using System.Reflection;
using Core.Logging;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;

namespace ShapezShifter.Hijack
{
    internal class SerializedGameScenarioInterceptor : IDisposable
    {
        private readonly IRewirerProvider RewirerProvider;
        private readonly ILogger Logger;
        private readonly ILHook IlHook;
        private readonly List<SerializedGameScenario> ModifiedScenarios = new();

        public SerializedGameScenarioInterceptor(IRewirerProvider rewirerProvider, ILogger logger)
        {
            RewirerProvider = rewirerProvider;
            Logger = logger;

            MethodInfo target = typeof(GameMode).GetMethod("From", BindingFlags.Static | BindingFlags.Public);
            IlHook = new ILHook(
                source: target!,
                manip: context =>
                {
                    ILCursor cursor = new(context);
                    cursor.GotoNext(MoveType.After, instruction => instruction.MatchCallvirt<IGameData>("GetRawScenario"));
                    cursor.EmitDelegate<Func<SerializedGameScenario, SerializedGameScenario>>(Postfix);
                });
        }

        public void Dispose()
        {
            IlHook.Dispose();
        }

        private SerializedGameScenario Postfix(SerializedGameScenario scenario)
        {
            if (ModifiedScenarios.Contains(scenario))
            {
                Logger.Info?.Log($"Serialized scenario {scenario.UniqueId} already modified, skipping {nameof(ISerializedGameScenarioRewirer)}s.");
                return scenario;
            }

            foreach (ISerializedGameScenarioRewirer rewirer in RewirerProvider.RewirersOfType<ISerializedGameScenarioRewirer>())
            {
                scenario = rewirer.ModifySerializedGameScenario(scenario);
            }
            
            ModifiedScenarios.Add(scenario);
            return scenario;
        }
    }
}
