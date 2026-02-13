using Odyssey.GrainInterfaces.Core.Services;

namespace Odyssey.GrainInterfaces.Core
{
    public interface IDataStorageCacheGrain<TData> : IGrain<string>, IGrainWithStringKey, IDataStorageGrainObserver<TData> where TData : class, new()
    {
        ValueTask<(TData Data, int Version)> GetDataAsync();
    }
}
