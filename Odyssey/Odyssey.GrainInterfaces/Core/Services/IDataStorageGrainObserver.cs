namespace Odyssey.GrainInterfaces.Core.Services
{
    public interface IDataStorageGrainObserver<TData> : IGrain
    {
        ValueTask Notify(TData data, int version);
    }
}
