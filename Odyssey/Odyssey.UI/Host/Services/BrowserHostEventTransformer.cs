using Microsoft.Extensions.DependencyInjection;
using Odyssey.Client.Authentication.Services;
using Odyssey.Core.Exceptions;
using Odyssey.Domain.Core.Services;
using Odyssey.Domain.Host.Services;
using Odyssey.Domain.Sessions.Events;
using Odyssey.UI.Core.Middlewares;
using Odyssey.UI.Core.Models;
using Odyssey.UI.Host.Components;

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
                _ => throw ExceptionFactory.CasesExhaustedException(inbound.Type, "event type")
            };
        }

        public async Task<string> TransformPartyEventAsync(PartyOutboundEvent outbound, string userId)
        {
            using var scope = scopeFactory.CreateScope(userId);
            var renderer = scope.ServiceProvider.GetRequiredService<IComponentStringRenderer>();

            return outbound switch
            {
                // TODO: this will be different when its the bottom sheet version..
                PartyDisbandedOutboundEvent disbandedEvent => await renderer.RenderComponentAsync(new HostPartyPanel
                {
                    HxSwapOob = true
                }),
                // TODO: make these more efficient, e.g. just remove the left member, just add the new member at the end, etc
                PartyMemberLeftOutboundEvent memberLeftEvent => await RegenerateHostPartyPanelAsync(renderer),
                PartyMemberJoinedOutboundEvent memberJoinedEvent => await RegenerateHostPartyPanelAsync(renderer),
                PartyMemberModifiedOutboundEvent memberModifiedEvent => await RegenerateHostPartyPanelAsync(renderer),
                _ => throw ExceptionFactory.CasesExhaustedException(outbound.GetType().Name, "event type")
            };
        }

        private static Task<string> RegenerateHostPartyPanelAsync(IComponentStringRenderer renderer) => renderer.RenderComponentAsync(new HostPartyPanel
        {
            HxSwapOob = true
        });
    }
}
