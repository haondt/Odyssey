namespace Odyssey.GrainInterfaces.Core.Services
{
    public interface IDataStorageCacheGrainFactory<TData> : IGrainFactory<string, IDataStorageCacheGrain<TData>> where TData : class, new()
    {
    }
}
