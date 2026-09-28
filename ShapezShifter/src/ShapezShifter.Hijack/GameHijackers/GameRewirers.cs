using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace ShapezShifter.Hijack
{
    [PublicAPI]
    public static class GameRewirers
    {
        internal static IEnumerable<IRewirer> Rewirers
        {
            get { return RewirersMap.Select(x => x.Value); }
        }

        internal static int Version;

        private static readonly SortedDictionary<RewirerHandle, IRewirer> RewirersMap = new();
        private static RewirerHandle LastHandle = RewirerHandle.Invalid;

        public static RewirerHandle AddRewirer<TRewirer>(TRewirer rewirer)
            where TRewirer : IRewirer
        {
            LastHandle = LastHandle.Next();
            RewirersMap.Add(key: LastHandle, value: rewirer);
            Version++;
            Debugging.Logger.Info?.Log($"Adding rewirer {rewirer.GetType().Name} with handle {LastHandle}");
            return LastHandle;
        }

        public static void RemoveRewirer(RewirerHandle rewirerHandle)
        {
            if (!RewirersMap.Remove(key: rewirerHandle, value: out IRewirer rewirers))
            {
                Debugging.Logger.Error?.Log($"Trying to remove handle that is no longer valid {rewirerHandle}");
                return;
            }

            Debugging.Logger.Info?.Log($"Removing rewirer with handle {rewirers.GetType().Name}");
            Version++;
        }

        public static void RemoveRewirer<TRewirer>(RewirerHandle rewirerHandle, out TRewirer rewirer)
        {
            bool removed = RewirersMap.Remove(key: rewirerHandle, value: out IRewirer ext);
            if (!removed)
            {
                throw new KeyNotFoundException();
            }

            Version++;
            if (ext is not TRewirer typedRewirer)
            {
                RewirersMap.Add(key: rewirerHandle, value: ext);
                throw new InvalidCastException();
            }

            rewirer = typedRewirer;
        }
    }
}
