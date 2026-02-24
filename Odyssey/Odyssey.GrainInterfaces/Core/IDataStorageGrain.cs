using Orleans.Concurrency;

namespace Odyssey.GrainInterfaces.Core
{
    public interface IDataStorageGrain<TData> : IGrain<string>, IGrainWithStringKey where TData : class, new()
    {
        [AlwaysInterleave]
        Task<(TData Data, int Version)> GetDataAsync();
        Task<int> SetDataAsync(TData data, int version);
    }
}
