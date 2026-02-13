using Microsoft.Extensions.Logging;
using Odyssey.GrainInterfaces.Core;
using Odyssey.GrainInterfaces.Core.Services;
using Orleans.Concurrency;

namespace Odyssey.Grains.Core
{
    [StatelessWorker]
    public class DataStorageCacheGrain<TData> : Grain, IDataStorageCacheGrain<TData>, IGrainObserver where TData : class, new()
    {
        private readonly IDataStorageGrain<TData> _grain;
        private readonly ILogger<DataStorageCacheGrain<TData>> _logger;
        private TData _cache = default!;
        private int _version = default!;
        private IGrainTimer _timer = default!;

        public DataStorageCacheGrain(
            ILogger<DataStorageCacheGrain<TData>> logger,
            IDataStorageCacheGrainFactory<TData> cacheGrainFactory,
            IDataStorageGrainFactory<TData> grainFactory)
        {
            _grain = grainFactory.GetGrain(cacheGrainFactory.GetIdentity(this));
            _logger = logger;
        }

        public override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            (_cache, _version) = await _grain.GetDataAsync();
            _timer = this.RegisterGrainTimer(
                static (self, ct) => self._grain.Subscribe(self.AsReference<IDataStorageCacheGrain<TData>>()).AsTask(),
                this,
                new(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)));
        }

        public override async Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
        {
            _timer.Dispose();
            try
            {
                await _grain.Unsubscribe(this.AsReference<IDataStorageCacheGrain<TData>>());
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Error occurred while attempting to unsubscribe from updates");
            }
        }

        public ValueTask<(TData Data, int Version)> GetDataAsync() => ValueTask.FromResult((_cache, _version));

        public ValueTask Notify(TData data, int version)
        {
            _cache = data;
            _version = version;
            return ValueTask.CompletedTask;
        }
    }
}
