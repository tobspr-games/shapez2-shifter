using System;
using System.Collections.Generic;
using Core.Factory;
using JetBrains.Annotations;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow
{
    /// <summary>
    /// Extension methods to facilitate appending mod save data to a save game 
    /// </summary>
    [PublicAPI]
    public static class ModSaveDataExtensions
    {
        private static readonly Dictionary<string, ActiveRewirer> Rewirers = new();

        /// <summary>
        /// <inheritdoc cref="AttachSaveData{T}(IMod, Core.Factory.IFactory{T})"/>
        /// </summary>
        /// <remarks>
        /// The default constructor is used when creating a default value for this type
        /// </remarks>
        /// <typeparam name="T">The type of data to save/load</typeparam>
        /// <param name="mod">The mod instance</param>
        public static void AttachSaveData<T>(this IMod mod)
            where T : new()
        {
            mod.AttachSaveData(new ParameterlessConstructionFactory<T>());
        }

        /// <summary>
        /// <inheritdoc cref="AttachSaveData{T}(IMod, Core.Factory.IFactory{T})"/>
        /// </summary>
        /// <typeparam name="T">The type of data to save/load</typeparam>
        /// <param name="mod">The mod instance</param>
        /// <param name="defaultDataFactory">A factory that creates a default instance of the data</param>
        public static void AttachSaveData<T>(this IMod mod, Func<T> defaultDataFactory)
            where T : new()
        {
            mod.AttachSaveData(new LambdaFactory<T>(defaultDataFactory));
        }

        /// <summary>
        /// Register a type T to be attached whenever a save game is serialized. The data is serialized as a JSON using
        /// the given mod information and type name to prevent conflicts.
        /// </summary>
        /// <typeparam name="T">The type of data to save/load</typeparam>
        /// <param name="mod">The mod instance</param>
        /// <param name="defaultDataFactory">A factory that creates a default instance of the data</param>
        public static void AttachSaveData<T>(this IMod mod, IFactory<T> defaultDataFactory)
        {
            string dataName = typeof(T).FullName;
            string assemblyName = mod.GetType().Assembly.GetName().Name;
            string fullName = $"{assemblyName}-{dataName}.json";
            string fileName = $"{fullName}.json";

            var rewirer = new ModSaveDataRewirer<T>(
                fileName: fileName,
                defaultDataFactory: defaultDataFactory,
                logger: Debugging.Logger);
            RewirerHandle handle = GameRewirers.AddRewirer(rewirer);
            var activeRewirer = new ActiveRewirer(handle: handle, saveDataRewirer: rewirer);
            Rewirers.Add(key: fullName, value: activeRewirer);
        }

        /// <summary>
        /// Subscribes an action to listen to the moment before a specific mod save data with type T is serialized
        /// </summary>
        /// <typeparam name="T">The type of mod save data that is attached</typeparam>
        /// <param name="mod">The mod instance</param>
        /// <param name="action">Listening action</param>
        /// <exception cref="KeyNotFoundException"> The type was never attached or already detached </exception>
        public static void RegisterToBeforeSaveDataSerialized<T>(this IMod mod, Action<T> action)
        {
            GetRewirer<T>(mod: mod, handle: out _).BeforeSaveDataSerialized.Register(action);
        }

        /// <summary>
        /// Subscribes an action to listen to the moment after a specific mod save data with type T is deserialized
        /// </summary>
        /// <typeparam name="T">The type of mod save data that is attached</typeparam>
        /// <param name="mod">The mod instance</param>
        /// <param name="action">Listening action</param>
        /// <exception cref="KeyNotFoundException"> The type was never attached or already detached </exception>
        public static void RegisterToAfterSaveDataDeserialized<T>(this IMod mod, Action<T> action)
        {
            GetRewirer<T>(mod: mod, handle: out _).AfterSaveDataDeserialized.Register(action);
        }

        /// <summary>
        /// Unsubscribes an action to listen to the moment before a specific mod save data with type T is serialized
        /// </summary>
        /// <typeparam name="T">The type of mod save data that is attached</typeparam>
        /// <param name="mod">The mod instance</param>
        /// <param name="action">Listening action</param>
        /// <exception cref="KeyNotFoundException"> The type was never attached or already detached </exception>
        /// <exception cref="InvalidOperationException"> The action was never registered or was already unregistered before </exception>
        public static void UnregisterToBeforeSaveDataSerialized<T>(this IMod mod, Action<T> action)
        {
            GetRewirer<T>(mod: mod, handle: out _).BeforeSaveDataSerialized.Unregister(action);
        }

        /// <summary>
        /// Unsubscribes an action to listen to the moment after a specific mod save data with type T is deserialized
        /// </summary>
        /// <typeparam name="T">The type of mod save data that is attached</typeparam>
        /// <param name="mod">The mod instance</param>
        /// <param name="action">Listening action</param>
        /// <exception cref="KeyNotFoundException"> The type was never attached or already detached </exception>
        /// <exception cref="InvalidOperationException"> The action was never registered or was already unregistered before </exception>
        public static void UnregisterToAfterSaveDataDeserialized<T>(this IMod mod, Action<T> action)
        {
            GetRewirer<T>(mod: mod, handle: out _).AfterSaveDataDeserialized.Unregister(action);
        }

        /// <param name="mod">The mod instance</param>
        ///   <typeparam name="T">The type of mod save data that is attached</typeparam>
        /// <returns>
        /// Returns a default object created by the factory:
        /// <list type="number">
        /// <item> before the deserialization occurred </item>
        /// <item> when the deserialization fails </item>
        /// <item> when the data was not present in the save game </item>
        /// <item> <see cref="ResetSaveDataToDefault{T}"/> is called </item>
        /// </list>
        /// Returns the deserialized object otherwise
        /// </returns>
        public static T GetSaveData<T>(this IMod mod)
        {
            return GetRewirer<T>(mod: mod, handle: out _).Data;
        }

        /// <summary>
        /// Resets the data of type T to its default value
        /// </summary>
        /// <param name="mod">The mod instance</param>
        /// <typeparam name="T">The type of mod save data that is attached</typeparam>
        public static void ResetSaveDataToDefault<T>(this IMod mod)
        {
            GetRewirer<T>(mod: mod, handle: out _).ResetToDefault();
        }

        /// <summary>
        /// Unregister a previously attached-to-be-saved type T.
        /// </summary>
        /// <typeparam name="T">The type of data to save/load</typeparam>
        /// <param name="mod">The mod instance</param>
        /// <exception cref="KeyNotFoundException"> The type was never attached or already detached before</exception>
        public static void DetachSaveData<T>(this IMod mod)
        {
            var rewirer = GetRewirer<T>(mod: mod, handle: out RewirerHandle handle);
            GameRewirers.RemoveRewirer(handle);
            rewirer.Dispose();
            Rewirers.Remove(mod.ResolveId<T>());
        }

        private static ModSaveDataRewirer<T> GetRewirer<T>(IMod mod, out RewirerHandle handle)
        {
            string id = mod.ResolveId<T>();
            ActiveRewirer activeRewirer = Rewirers[id];
            handle = activeRewirer.Handle;
            return (ModSaveDataRewirer<T>)activeRewirer.SaveDataRewirer;
        }

        private static string ResolveId<T>(this IMod mod)
        {
            string dataName = typeof(T).FullName;
            string assemblyName = mod.GetType().Assembly.GetName().Name;
            string fullName = $"{assemblyName}-{dataName}";
            return fullName;
        }

        private readonly struct ActiveRewirer
        {
            internal readonly RewirerHandle Handle;
            internal readonly ISaveDataRewirer SaveDataRewirer;

            public ActiveRewirer(RewirerHandle handle, ISaveDataRewirer saveDataRewirer)
            {
                Handle = handle;
                SaveDataRewirer = saveDataRewirer;
            }
        }
    }
}
