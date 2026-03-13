using Odyssey.Domain.Core.Models;
using Odyssey.GrainInterfaces.Core;
using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;
using Orleans.Storage;


namespace Odyssey.Grains.Core
{

    public class DataStorageGrain<TData> : Grain, IDataStorageGrain<TData> where TData : IDataStorageData<TData>
    {
        private readonly IPersistentState<DataStorageModel<TData>> _state;
        private readonly JsonUtils _jsonUtils;

        public DataStorageGrain(
            IGrainContext context,
            IPersistentStateFactory persistentStateFactory,
            JsonUtils jsonUtils)
        {
            _state = persistentStateFactory.Create<DataStorageModel<TData>>(context, new PersistentStateConfiguration
            {
                StateName = $"{nameof(DataStorageGrain<>)}+{SimpleTypeSerializer.TypeToString(typeof(TData))}",
                StorageName = GrainConstants.GrainStorage
            });
            _jsonUtils = jsonUtils;
        }

        public override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            if (!_state.RecordExists)
                _state.State.Data = TData.Factory();
            return base.OnActivateAsync(cancellationToken);
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
        public Task ClearDataAsync() => _state.ClearStateAsync();

        public Task<(TData Data, int Version)> GetDataAsync() => Task.FromResult((_jsonUtils.CloneObject(_state.State.Data), _state.State.Version));
    }



}
