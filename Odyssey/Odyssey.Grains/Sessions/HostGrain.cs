using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions;

namespace Odyssey.Grains.Sessions
{
    public class HostGrain : Grain, IHostGrain
    {
        private readonly IHostPartyGrain _party;

        public HostGrain(IGrainFactory<string, IHostGrain> grainFactory,
            IGrainFactory<string, IHostPartyGrain> partyGrainFactory)
        {
            var id = grainFactory.GetIdentity(this);
            _party = partyGrainFactory.GetGrain(id);
        }
    }
}
