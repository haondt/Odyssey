namespace Odyssey.Domain.Core.Services
{
    public interface ICachedDataService<T> where T : class, new()
    {
        ValueTask<(T Data, int Version)> GetDataAsync(string key);
        Task<int> SetDataAsync(string key, T data, int version);
    }
}
