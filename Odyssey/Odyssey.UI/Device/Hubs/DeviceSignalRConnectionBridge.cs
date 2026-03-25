using Haondt.Core.Models;
using Microsoft.AspNetCore.SignalR;
using Odyssey.Domain.Core.Events;
using Odyssey.Domain.Core.Services;
using Odyssey.Domain.Device.Events;
using Odyssey.Domain.Device.Services;
using Odyssey.Domain.Sessions.Events;
using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.UI.Device.Models;
using Odyssey.UI.Device.Services;
using Odyssey.UI.Host.Models;
using Orleans.Streams;

namespace Odyssey.UI.Device.Hubs
{

    public class DeviceSignalRConnectionBridge<TInbound, TOutbound, THub>(
        string connectionId,
        DeviceClientType clientType,
        Guid displayId,
        IHubContext<THub, IDeviceHubReceiver<TOutbound>> context,
        IEventTransformerRegistry transformerRegistry,
        IClusterClient clusterClient) : IDeviceSignalRConnectionBridge<TInbound> where THub : DeviceHub<TInbound, TOutbound>

    {
        private readonly IDeviceEventTransformer<TInbound, TOutbound> _transformer = transformerRegistry.GetTransformer<IDeviceEventTransformer<TInbound, TOutbound>>(clientType);
        private Optional<StreamSubscriptionHandle<SignalROutboundEvent>> _handle;

        public Guid DeviceId => displayId;

        public async Task OnConnectedAsync()
        {
            var stream = clusterClient
                .GetStreamProvider(GrainConstants.SignalRStreams)
                .GetStream<SignalROutboundEvent>(StreamId.Create(GrainConstants.DeviceEventsStreamNamespace, displayId));

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
                    case DevicePartyOutboundEvent partyEvent:
                        {
                            var payload = await _transformer.TransformDevicePartyEventAsync(partyEvent, displayId);
                            // i am making the perhaps unwise decision to use ReceivePartyEvent for both PartyOutboundEvents and DevicePartyOutboundEvents
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
