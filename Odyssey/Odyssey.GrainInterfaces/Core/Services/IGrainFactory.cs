namespace Odyssey.GrainInterfaces.Core.Services
{
    public interface IGrainFactory<TIdentity, TGrain> where TGrain : IGrain<TIdentity>
    {
        TGrain GetGrain(TIdentity key);
        TIdentity GetIdentity(TGrain grain);

    }
}
