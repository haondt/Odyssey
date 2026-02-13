using Odyssey.GrainInterfaces.Core.Services;
using Orleans.Concurrency;

namespace Odyssey.GrainInterfaces.Core
{
    public interface IDataStorageGrain<TData> : IGrain<string>, IGrainWithStringKey where TData : class, new()
    {
        [AlwaysInterleave]
        ValueTask<(TData Data, int Version)> GetDataAsync();
        Task<int> SetDataAsync(TData data, int version);
        public ValueTask Subscribe(IDataStorageGrainObserver<TData> observer);
        public ValueTask Unsubscribe(IDataStorageGrainObserver<TData> observer);
    }
}
