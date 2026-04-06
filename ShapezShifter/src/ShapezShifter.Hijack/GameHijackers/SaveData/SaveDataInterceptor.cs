using System;
using Core.Logging;
using Game.Core.Modding;
using MonoMod.RuntimeDetour;
using ShapezShifter.SharpDetour;

namespace ShapezShifter.Hijack
{
    internal class SaveDataInterceptor : IDisposable
    {
        private readonly IRewirerProvider RewirerProvider;
        private readonly ILogger Logger;
        private readonly Hook SerializeSavegameHook;
        private readonly Hook DeserializePlayerHook;

        public SaveDataInterceptor(IRewirerProvider rewirerProvider, ILogger logger)
        {
            RewirerProvider = rewirerProvider;
            Logger = logger;

            SerializeSavegameHook =
                DetourHelper.CreatePostfixHook<Savegame, ISavegameBlobWriter, IModdingFrameworkEnvironment>(
                    original: (save, reader, movEnv) => save.Serialize(reader, movEnv),
                    postfix: OnSerializeSavePostfix);

            DeserializePlayerHook = DetourHelper.CreateStaticPostfixHook<Savegame, SavegameBlobReader>(
                original: reader => Savegame.Deserialize(reader),
                postfix: OnDeserializeSavePostfix);
        }

        private void OnSerializeSavePostfix(
            Savegame save,
            ISavegameBlobWriter writer,
            IModdingFrameworkEnvironment modEnv)
        {
            var saveDataRewirers = RewirerProvider.RewirersOfType<ISaveDataRewirer>();

            foreach (ISaveDataRewirer rewirer in saveDataRewirers)
            {
                try
                {
                    rewirer.OnSave(writer);
                }
                catch (Exception ex)
                {
                    Logger.Error?.Log($"Error in mod save data rewirer {rewirer}: {ex}");
                }
            }
        }

        private void OnDeserializeSavePostfix(SavegameBlobReader reader)
        {
            Logger.Info?.Log("Intercepting savegame deserialization for mod data");

            // Get all save data rewirers
            var saveDataRewirers = RewirerProvider.RewirersOfType<ISaveDataRewirer>();

            foreach (ISaveDataRewirer rewirer in saveDataRewirers)
            {
                try
                {
                    rewirer.OnLoad(reader);
                }
                catch (Exception ex)
                {
                    Logger.Error?.Log($"Error in mod load data rewirer: {ex}");
                }
            }
        }

        public void Dispose()
        {
            SerializeSavegameHook?.Dispose();
            DeserializePlayerHook?.Dispose();
        }
    }
}
