namespace Odyssey.GrainInterfaces.Core.Services
{
    public class GrainLeaseGrainFactory(IGrainFactory grainFactory) : IGrainLeaseGrainFactory
    {
        public IGrainLeaseGrain GetGrain(string key)
        {
            return grainFactory.GetGrain<IGrainLeaseGrain>(key);
        }

        public string GetIdentity(IGrainLeaseGrain grain)
        {
            return grain.GetPrimaryKeyString();
        }
    }
}
