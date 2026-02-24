using Odyssey.GrainInterfaces.Core;
using Odyssey.GrainInterfaces.Core.Models;
using Orleans.Storage;

namespace Odyssey.Grains.Core
{
    public class DataStorageGrain<TData> : Grain, IDataStorageGrain<TData> where TData : class, new()
    {
        private readonly IPersistentState<DataStorageModel<TData>> _state;

        public DataStorageGrain(
            IGrainContext context,
            IPersistentStateFactory persistentStateFactory)
        {
            _state = persistentStateFactory.Create<DataStorageModel<TData>>(context, new PersistentStateConfiguration
            {
                StateName = $"{nameof(DataStorageGrain<>)}+{typeof(TData).Name}",
                StorageName = GrainConstants.GrainStorage
            });
        }

        public async Task<int> SetDataAsync(TData data, int version)
        {
            if (version > _state.State.Version)
                await _state.ReadStateAsync();

            if (version != _state.State.Version)
                throw new InconsistentStateException($"Given version {version} is different than expected version {_state.State.Version}");
            _state.State.Version += 1;
            _state.State.Data = data;
            await _state.WriteStateAsync();
            return _state.State.Version;
        }

        public Task<(TData Data, int Version)> GetDataAsync() => Task.FromResult((_state.State.Data, _state.State.Version));

    }
}
