using Haondt.Core.Models;
using Microsoft.AspNetCore.SignalR;
using Odyssey.Domain.Core.Events;
using Odyssey.Domain.Core.Services;
using Odyssey.Domain.Display.Events;
using Odyssey.Domain.Display.Services;
using Odyssey.Domain.Sessions.Events;
using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.UI.Display.Services;
using Odyssey.UI.Host.Models;
using Orleans.Streams;

namespace Odyssey.UI.Display.Hubs
{

    public class DisplaySignalRConnectionBridge<THub>(
        string connectionId,
        Guid displayId,
        IHubContext<THub, IDisplayHubReceiver<string>> context,
        IEventTransformerRegistry transformerRegistry,
        IClusterClient clusterClient) : IDisplaySignalRConnectionBridge<HtmxSignalRMessage> where THub : DisplayHub
    {
        private readonly IDisplayEventTransformer<HtmxSignalRMessage, string> _transformer = transformerRegistry.GetTransformer<IDisplayEventTransformer<HtmxSignalRMessage, string>>();
        private Optional<StreamSubscriptionHandle<SignalROutboundEvent>> _handle;

        public Guid DisplayId => displayId;

        public async Task OnConnectedAsync()
        {
            var stream = clusterClient
                .GetStreamProvider(GrainConstants.SignalRStreams)
                .GetStream<SignalROutboundEvent>(StreamId.Create(GrainConstants.DisplayEventsStreamNamespace, displayId));

            _handle = await stream.SubscribeAsync(async (evt, _) =>
            {
                switch (evt)
                {
                    case PartyOutboundEvent partyEvent:
                        {
                            var payload = await _transformer.TransformPartyEventAsync(partyEvent, displayId);
                            await context.Clients.Client(connectionId).ReceivePartyEvent(payload);
                            break;
                        }
                    case DisplayPartyOutboundEvent partyEvent:
                        {
                            var payload = await _transformer.TransformDisplayPartyEventAsync(partyEvent, displayId);
                            // i am making the perhaps unwise decision to use ReceivePartyEvent for both PartyOutboundEvents and DisplayPartyOutboundEvents
                            await context.Clients.Client(connectionId).ReceivePartyEvent(payload);
                            break;
                        }
                }
            });
        }

        public async Task OnDisconnectedAsync()
        {
            if (_handle.TryGetValue(out var handle))
                await handle.UnsubscribeAsync();
        }
    }
}
