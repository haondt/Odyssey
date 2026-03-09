using Haondt.Web.Components;
using Haondt.Web.UI.Components.Components;
using Haondt.Web.UI.Components.Containers;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.Client.Authentication.Services;
using Odyssey.Core.Exceptions;
using Odyssey.Domain.Core.Services;
using Odyssey.Domain.Display.Events;
using Odyssey.Domain.Display.Services;
using Odyssey.Domain.Sessions.Events;
using Odyssey.UI.Core.Models;
using Odyssey.UI.Display.Components;
using Odyssey.UI.Host.Models;

namespace Odyssey.UI.Display.Services
{
    public class DisplayEventTransformer(ISignalRScopeFactory scopeFactory) : IDisplayEventTransformer<HtmxSignalRMessage, string>
    {
        public async Task<string> TransformPartyEventAsync(PartyOutboundEvent outbound, Guid displayId)
        {
            using var scope = scopeFactory.CreateDisplayScope(displayId);
            var renderer = scope.ServiceProvider.GetRequiredService<IComponentStringRenderer>();

            return outbound switch
            {
                PartyDisbandedOutboundEvent disbandedEvent => await renderer.RenderComponentAsync(new AppendComponentLayout
                {
                    Components = new()
                    {
                        new DisplayBottomSheetContent
                        {
                            SignalRSubscribe = false
                        },
                        new NotificationDialog
                        {
                            Title = "Party disbanded",
                            Message = "Click ok to return to the join party screen",
                            OkHxGet = OdysseyRoutes.Display.Party.Index,
                            OkCloseModal = false
                        },
                        Trigger.Create("close", $"#{BottomSheetContainer.Id}")
                    }
                }),
                PartyMemberLeftOutboundEvent memberLeftEvent => await RegeneratePartyPanelAsync(renderer),
                PartyMemberJoinedOutboundEvent memberJoinedEvent => await RegeneratePartyPanelAsync(renderer),
                _ => throw ExceptionFactory.CasesExhaustedException(outbound.GetType().Name, "event type")
            };
        }

        private static Task<string> RegeneratePartyPanelAsync(IComponentStringRenderer renderer) => renderer.RenderComponentAsync(new DisplayBottomSheetContent
        {
            SignalRSubscribe = false
        });

        public async Task<string> TransformDisplayPartyEventAsync(DisplayPartyOutboundEvent outbound, Guid displayId)
        {
            using var scope = scopeFactory.CreateDisplayScope(displayId);
            var renderer = scope.ServiceProvider.GetRequiredService<IComponentStringRenderer>();

            return outbound switch
            {
                DisplaySelfLeftPartyOutboundEvent selfLeftEvent => await renderer.RenderComponentAsync(new AppendComponentLayout
                {
                    Components = new()
                    {
                        new DisplayBottomSheetContent
                        {
                            SignalRSubscribe = false
                        },
                        Trigger.Create("close", $"#{BottomSheetContainer.Id}")
                    }
                }),
                _ => throw ExceptionFactory.CasesExhaustedException(outbound.GetType().Name, "event type")
            };

        }
    }
}
