using Microsoft.Extensions.DependencyInjection;
using Odyssey.Client.Authentication.Services;
using Odyssey.Domain.Core.Services;
using Odyssey.Domain.Host.Services;
using Odyssey.Domain.Sessions.Events;
using Odyssey.UI.Host.Components;
using Odyssey.UI.Host.Models;

namespace Odyssey.UI.Host.Services
{
    public class BrowserHostEventTransformer(ISignalRScopeFactory scopeFactory) : IHostEventTransformer<HtmxSignalRMessage, string>
    {

        public PartyInboundEvent TransformPartyEvent(HtmxSignalRMessage inbound, string connectionId)
        {
            return inbound.Type switch
            {
                PartyDisbandedInboundEvent.Type => new PartyDisbandedInboundEvent() { OriginConnectionId = connectionId },
                PartyMemberLeftInboundEvent.Type => throw new NotImplementedException(),
                _ => throw new ArgumentException($"Unknown event type \"{inbound.Type}\"")
            };
        }

        public async Task<string> TransformPartyEventAsync(PartyOutboundEvent outbound, string userId)
        {
            using var scope = scopeFactory.CreateScope(userId);
            var renderer = scope.ServiceProvider.GetRequiredService<IComponentStringRenderer>();

            return outbound switch
            {
                PartyDisbandedOutboundEvent disbandedEvent => await renderer.RenderComponentAsync(new HostPartyPanel
                {
                    HxSwapOob = true
                }),
                PartyMemberLeftOutboundEvent memberLeftEvent => $"<p>Member {memberLeftEvent.MemberId} left the party.</p>",
                _ => throw new ArgumentException($"Unknown event type \"{outbound.GetType()}\"")
            };
        }

    }
}
