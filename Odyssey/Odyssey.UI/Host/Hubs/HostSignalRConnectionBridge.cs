using Haondt.Core.Models;
using Microsoft.AspNetCore.SignalR;
using Odyssey.Domain.Core.Events;
using Odyssey.Domain.Core.Services;
using Odyssey.Domain.Host.Services;
using Odyssey.Domain.Sessions.Events;
using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.UI.Host.Models;
using Odyssey.UI.Host.Services;
using Orleans.Streams;

namespace Odyssey.UI.Host.Hubs
{

    public class HostSignalRConnectionBridge<TInbound, TOutbound, THub>(
        string connectionId,
        HostClientType clientType,
        string userId,
        IHubContext<THub, IHostHubReceiver<TOutbound>> context,
        IEventTransformerRegistry transformerRegistry,
        IClusterClient clusterClient) : IHostSignalRConnectionBridge<TInbound> where THub : HostHub<TInbound, TOutbound>
    {
        private readonly IHostEventTransformer<TInbound, TOutbound> _transformer = transformerRegistry.GetTransformer<IHostEventTransformer<TInbound, TOutbound>>(clientType);
        private Optional<StreamSubscriptionHandle<SignalROutboundEvent>> _handle;

        public async Task OnConnectedAsync()
        {
            var stream = clusterClient
                .GetStreamProvider(GrainConstants.SignalRStreams)
                .GetStream<SignalROutboundEvent>(StreamId.Create(GrainConstants.HostEventsStreamNamespace, userId));

            _handle = await stream.SubscribeAsync(async (evt, _) =>
            {
                switch (evt)
                {
                    case PartyOutboundEvent partyEvent:
                        var payload = await _transformer.TransformPartyEventAsync(partyEvent, userId);
                        await context.Clients.Client(connectionId).ReceivePartyEvent(payload);
                        break;
                }
            });
        }

        public async Task OnDisconnectedAsync()
        {
            if (_handle.TryGetValue(out var handle))
                await handle.UnsubscribeAsync();
        }

        public Task SendPartyEvent(TInbound body)
        {
            var partyEvent = _transformer.TransformPartyEvent(body, connectionId);
            // todo... need to think about it more. I suppose the natural thing to do would be like
            // IHostGrain grain = grainFactory.GetGrain(userId)
            // grain.ReceivePartyEvent(partyEvent)

            throw new NotImplementedException();
        }
    }
}
