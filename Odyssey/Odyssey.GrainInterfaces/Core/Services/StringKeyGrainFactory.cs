namespace Odyssey.GrainInterfaces.Core.Services
{
    public class StringKeyGrainFactory<T>(IGrainFactory grainFactory) : IGrainFactory<string, T> where T : IGrain<string>, IGrainWithStringKey
    {
        public T GetGrain(string key) => grainFactory.GetGrain<T>(key);
        public string GetIdentity(T grain) => grain.GetPrimaryKeyString();
    }
}
