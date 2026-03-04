using Odyssey.Domain.Core.Events;
using Odyssey.Domain.Sessions.Events;
using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions;
using Orleans.Streams;

namespace Odyssey.Grains.Sessions
{
    public class HostGrain : Grain, IHostGrain
    {
        private readonly IHostPartyGrain _party;
        private readonly IAsyncStream<SignalROutboundEvent> _hostEventStream;

        public HostGrain(IGrainFactory<string, IHostGrain> grainFactory,
            IGrainFactory<string, IHostPartyGrain> partyGrainFactory)
        {
            var id = grainFactory.GetIdentity(this);
            _party = partyGrainFactory.GetGrain(id);
            _hostEventStream = this.GetStreamProvider(GrainConstants.SignalRStreams)
                .GetStream<SignalROutboundEvent>(GrainConstants.HostEventsStreamNamespace, id);
        }

        public Task NotifyPartyDisbandedAsync(string partyId)
        {
            return _hostEventStream.OnNextAsync(new PartyDisbandedOutboundEvent { PartyId = partyId });
        }
    }
}
