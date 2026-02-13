using Microsoft.Extensions.Logging;
using Odyssey.GrainInterfaces.Core;
using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;
using Orleans.Storage;
using Orleans.Utilities;

namespace Odyssey.Grains.Core
{
    public class DataStorageGrain<TData> : Grain, IDataStorageGrain<TData> where TData : class, new()
    {
        private readonly ILogger<DataStorageGrain<TData>> _logger;
        private readonly IPersistentState<DataStorageModel<TData>> _state;
        private readonly ObserverManager<IDataStorageGrainObserver<TData>> _subsManager;

        public DataStorageGrain(
            IGrainContext context,
            IPersistentStateFactory persistentStateFactory,
            ILogger<DataStorageGrain<TData>> logger)
        {
            _logger = logger;
            _state = persistentStateFactory.Create<DataStorageModel<TData>>(context, new PersistentStateConfiguration
            {
                StateName = $"{nameof(DataStorageGrain<>)}+{typeof(TData).Name}",
                StorageName = GrainConstants.GrainStorage
            });
            _subsManager = new ObserverManager<IDataStorageGrainObserver<TData>>(TimeSpan.FromMinutes(5), logger);
        }

        public async Task<int> SetDataAsync(TData data, int version)
        {
            if (version > _state.State.Version)
                await _state.ReadStateAsync();

            if (version == _state.State.Version)
            {
                _state.State.Version += 1;
                _state.State.Data = data;
                await WriteStateAndReadOnFailureAsync();
                try
                {
                    await _subsManager.Notify(s => s.Notify(_state.State.Data, _state.State.Version).AsTask());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception occurred while notifying observers.");
                }
                return _state.State.Version;
            }

            throw new InconsistentStateException($"Given version {version} is different than expected version {_state.State.Version}");
        }

        public ValueTask<(TData Data, int Version)> GetDataAsync() => ValueTask.FromResult((_state.State.Data, _state.State.Version));

        private async Task WriteStateAndReadOnFailureAsync()
        {
            try
            {
                await _state.WriteStateAsync();
            }
            catch (InconsistentStateException)
            {
                await _state.ReadStateAsync();
                throw;
            }
        }

        public ValueTask Subscribe(IDataStorageGrainObserver<TData> observer)
        {
            _subsManager.Subscribe(observer, observer);
            return ValueTask.CompletedTask;
        }

        public ValueTask Unsubscribe(IDataStorageGrainObserver<TData> observer)
        {
            _subsManager.Unsubscribe(observer);
            return ValueTask.CompletedTask;
        }
    }
}
