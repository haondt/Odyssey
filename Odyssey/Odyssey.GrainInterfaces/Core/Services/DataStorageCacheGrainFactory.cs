namespace Odyssey.GrainInterfaces.Core.Services
{
    public class DataStorageCacheGrainFactory<TData>(IGrainFactory grainFactory) : IDataStorageCacheGrainFactory<TData> where TData : class, new()
    {
        public IDataStorageCacheGrain<TData> GetGrain(string key)
        {
            return grainFactory.GetGrain<IDataStorageCacheGrain<TData>>(key);
        }

        public string GetIdentity(IDataStorageCacheGrain<TData> grain)
        {
            return grain.GetPrimaryKeyString();
        }
    }
}
