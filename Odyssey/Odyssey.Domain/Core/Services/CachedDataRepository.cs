using Odyssey.GrainInterfaces.Core.Services;

namespace Odyssey.Domain.Core.Services
{
    public class CachedDataRepository<T>(
        IDataStorageGrainFactory<T> grainFactory) : ICachedDataRepository<T> where T : IDataStorageData<T>
    {
        public Task<(T Data, int Version)> GetDataAsync(string key)
        {
            var cacheGrain = grainFactory.GetGrain(key);
            return cacheGrain.GetDataAsync();
        }

        public Task<int> SetDataAsync(string key, T data, int version)
        {
            var grain = grainFactory.GetGrain(key);
            return grain.SetDataAsync(data, version);
        }
        public Task ClearDataAsync(string key)
        {
            var grain = grainFactory.GetGrain(key);
            return grain.ClearDataAsync();
        }
    }
}
