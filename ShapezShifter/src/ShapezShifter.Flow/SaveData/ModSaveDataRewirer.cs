using System;
using System.IO;
using Core.Events;
using Core.Factory;
using Core.Logging;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow
{
    public class ModSaveDataRewirer<T> : ISaveDataRewirer, IDisposable
    {
        private readonly string FileName;
        private readonly IFactory<T> DefaultDataFactory;
        private readonly ILogger Logger;
        private bool Disposed = false;

        public T Data { get; private set; }

        public IEvent<T> BeforeSaveDataSerialized
        {
            get { return _BeforeSaveDataSerialized; }
        }

        private readonly MultiRegisterEvent<T> _BeforeSaveDataSerialized = new();

        public IEvent<T> AfterSaveDataDeserialized
        {
            get { return _AfterSaveDataDeserialized; }
        }

        private readonly MultiRegisterEvent<T> _AfterSaveDataDeserialized = new();

        public ModSaveDataRewirer(string fileName, IFactory<T> defaultDataFactory, ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(message: "Filename cannot be null or empty", paramName: nameof(fileName));
            }
            
            fileName = Path.ChangeExtension(path: fileName, extension: "json");
            FileName = fileName;
            DefaultDataFactory = defaultDataFactory;
            Logger = logger;
            Data = defaultDataFactory.Produce();
        }

        public void ResetToDefault()
        {
            Data = DefaultDataFactory.Produce();
        }

        void ISaveDataRewirer.OnSave(ISavegameBlobWriter writer)
        {
            try
            {
                _BeforeSaveDataSerialized?.Invoke(Data);
                writer.WriteObjectAsJson(filename: FileName, obj: Data);
            }
            catch (Exception ex)
            {
                Logger?.Error?.Log($"Failed to save mod data for {FileName}: {ex}");
            }
        }

        void ISaveDataRewirer.OnLoad(SavegameBlobReader reader)
        {
            try
            {
                Data = reader.ReadObjectFromJson<T>(FileName);
                _AfterSaveDataDeserialized?.Invoke(Data);
            }
            catch (Exception)
            {
                Data = DefaultDataFactory.Produce();
                Logger?.Info?.Log($"No existing save data found for {FileName}, using defaults");
            }
        }

        public void Dispose()
        {
            if (Disposed)
            {
                return;
            }
            Disposed = true;
            _BeforeSaveDataSerialized?.Dispose();
            _AfterSaveDataDeserialized?.Dispose();
        }
    }
}
