using Odyssey.GrainInterfaces.Core.Services;

namespace Odyssey.Domain.Core.Services
{
    public class CachedDataRepository<T>(
        IDataStorageCacheGrainFactory<T> cacheGrainFactory,
        IDataStorageGrainFactory<T> grainFactory) : ICachedDataRepository<T> where T : class, new()
    {
        public ValueTask<(T Data, int Version)> GetDataAsync(string key)
        {
            var cacheGrain = cacheGrainFactory.GetGrain(key);
            return cacheGrain.GetDataAsync();
        }

        public Task<int> SetDataAsync(string key, T data, int version)
        {
            var grain = grainFactory.GetGrain(key);
            return grain.SetDataAsync(data, version);
        }
    }
}
