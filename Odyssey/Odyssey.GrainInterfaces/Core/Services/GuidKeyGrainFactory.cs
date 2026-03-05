namespace Odyssey.GrainInterfaces.Core.Services
{
    public class GuidKeyGrainFactory<T>(IGrainFactory grainFactory) : IGrainFactory<Guid, T> where T : IGrain<Guid>, IGrainWithGuidKey
    {
        public T GetGrain(Guid key) => grainFactory.GetGrain<T>(key);
        public Guid GetIdentity(T grain) => grain.GetPrimaryKey();
    }
}
