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
        
        public static void AddRewirer<TRewirer>(RewirerHandle reservedHandle, TRewirer rewirer)
            where TRewirer : IRewirer
        {
            if (RewirersMap.ContainsKey(reservedHandle))
            {
                Debugging.Logger.Error?.Log($"Tried adding rewirer {rewirer.GetType().Name} with duplicated handle {reservedHandle}");
                return;
            }
            RewirersMap.Add(key: reservedHandle, value: rewirer);
            Version++;
            Debugging.Logger.Info?.Log($"Adding rewirer {rewirer.GetType().Name} with reserved handle {reservedHandle}");
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

        /// <summary>
        /// Reserves a <c>RewirerHandle</c> that can be used in <c>AddRewirer(RewirerHandle, TRewirer)</c>
        /// to register a rewirer with a known handle. This is useful to ensure rewirers are invoked in
        /// a consistent order even when registering and removing a rewirer multiple times.  
        /// </summary>
        public static RewirerHandle ReserveHandle()
        {
            LastHandle = LastHandle.Next();
            return LastHandle;
        }
    }
}
