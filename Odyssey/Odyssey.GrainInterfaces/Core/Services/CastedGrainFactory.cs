namespace Odyssey.GrainInterfaces.Core.Services
{
    public class CastedGrainFactory<TIdentity, TGrain, TReference>(IGrainFactory<TIdentity, TGrain> grainFactory) : ICastedGrainFactory<TIdentity, TReference>
        where TGrain : IGrain<TIdentity>, TReference
    {
        public TReference GetGrain(TIdentity key)
        {
            var grain = grainFactory.GetGrain(key);
            return grain;
        }
    }
}
