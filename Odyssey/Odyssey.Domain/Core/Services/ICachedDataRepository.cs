using Odyssey.GrainInterfaces.Core.Services;

namespace Odyssey.Domain.Core.Services
{
    public interface ICachedDataRepository<T> where T : IDataStorageData<T>
    {
        Task<(T Data, int Version)> GetDataAsync(string key);
        Task<int> SetDataAsync(string key, T data, int version);
        Task ClearDataAsync(string key);
    }
}
