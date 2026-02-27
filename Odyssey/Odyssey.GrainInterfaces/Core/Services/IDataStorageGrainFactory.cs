namespace Odyssey.GrainInterfaces.Core.Services
{
    public interface IDataStorageGrainFactory<TData> : IGrainFactory<string, IDataStorageGrain<TData>> where TData : IDataStorageData<TData>
    {
    }
}
