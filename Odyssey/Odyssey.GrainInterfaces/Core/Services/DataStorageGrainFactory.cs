namespace Odyssey.GrainInterfaces.Core.Services
{
    public class DataStorageGrainFactory<TData>(IGrainFactory grainFactory) : IDataStorageGrainFactory<TData> where TData : IDataStorageData<TData>
    {
        public IDataStorageGrain<TData> GetGrain(string key)
        {
            return grainFactory.GetGrain<IDataStorageGrain<TData>>(key);
        }

        public string GetIdentity(IDataStorageGrain<TData> grain)
        {
            return grain.GetPrimaryKeyString();
        }
    }
}
