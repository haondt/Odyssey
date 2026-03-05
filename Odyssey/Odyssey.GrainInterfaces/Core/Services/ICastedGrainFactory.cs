namespace Odyssey.GrainInterfaces.Core.Services
{
    public interface ICastedGrainFactory<TIdentity, TReference>
    {
        TReference GetGrain(TIdentity key);
    }
}
