using Haondt.Core.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
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
        IClusterClient clusterClient,
        ILogger<HostSignalRConnectionBridge<TInbound, TOutbound, THub>> logger) : IHostSignalRConnectionBridge<TInbound> where THub : HostHub<TInbound, TOutbound>
    {
        private readonly IHostEventTransformer<TInbound, TOutbound> _transformer = transformerRegistry.GetTransformer<IHostEventTransformer<TInbound, TOutbound>>(clientType);
        private Optional<StreamSubscriptionHandle<SignalROutboundEvent>> _handle;

        public string UserId => userId;

        // TODO: on connected we should fire an event to make something bounce back to update a stale page on the client side
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
                        if (logger.IsEnabled(LogLevel.Debug))
                            logger.LogDebug("Received outbound party event {EventType} for Host {HostId}", partyEvent.GetType().Name, userId);
                        var payload = await _transformer.TransformPartyEventAsync(partyEvent, userId);
                        await context.Clients.Client(connectionId).ReceivePartyEvent(payload);
                        break;
                }
            });

            logger.LogDebug("Connected to Orleans stream for Host {HostId}", userId);
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
