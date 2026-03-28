using Haondt.Web.Components;
using Haondt.Web.UI.Components.Components;
using Haondt.Web.UI.Components.Containers;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.Client.Authentication.Services;
using Odyssey.Core.Exceptions;
using Odyssey.Domain.Core.Services;
using Odyssey.Domain.Device.Events;
using Odyssey.Domain.Device.Services;
using Odyssey.Domain.Sessions.Events;
using Odyssey.UI.Core.Middlewares;
using Odyssey.UI.Core.Models;
using Odyssey.UI.Device.Components;

namespace Odyssey.UI.Device.Services
{
    public class DeviceEventTransformer(ISignalRScopeFactory scopeFactory) : IDeviceEventTransformer<HtmxSignalRMessage, string>
    {
        public async Task<string> TransformPartyEventAsync(PartyOutboundEvent outbound, Guid deviceId)
        {
            using var scope = scopeFactory.CreateDeviceScope(deviceId);
            var renderer = scope.ServiceProvider.GetRequiredService<IComponentStringRenderer>();

            return outbound switch
            {
                PartyDisbandedOutboundEvent disbandedEvent => await renderer.RenderComponentAsync(new AppendComponentLayout
                {
                    Components = new()
                    {
                        new DeviceBottomSheetContent
                        {
                            SignalRSubscribe = false
                        },
                        new NotificationDialog
                        {
                            Title = "Party disbanded",
                            Message = "Click ok to return to the join party screen",
                            OkHxGet = OdysseyRoutes.Device.Party.Index,
                            OkCloseModal = false
                        },
                        Trigger.Create("close", $"#{BottomSheetContainer.Id}")
                    }
                }),
                PartyMemberLeftOutboundEvent memberLeftEvent => await RegeneratePartyPanelAsync(renderer),
                PartyMemberJoinedOutboundEvent memberJoinedEvent => await RegeneratePartyPanelAsync(renderer),
                PartyMemberModifiedOutboundEvent memberLeftEvent => await RegeneratePartyPanelAsync(renderer),
                RemovedFromPartyOutboundEvent removedEvent => await renderer.RenderComponentAsync(new AppendComponentLayout
                {
                    Components = new()
                    {
                        new DeviceBottomSheetContent
                        {
                            SignalRSubscribe = false
                        },
                        new NotificationDialog
                        {
                            Title = "You have been removed from the party",
                            Message = "Click ok to return to the join party screen",
                            OkHxGet = OdysseyRoutes.Display.Party.Index,
                            OkCloseModal = false
                        },
                        Trigger.Create("close", $"#{BottomSheetContainer.Id}")
                    }
                }),
                _ => throw ExceptionFactory.CasesExhaustedException(outbound.GetType().Name, "event type")
            };
        }

        private static Task<string> RegeneratePartyPanelAsync(IComponentStringRenderer renderer) => renderer.RenderComponentAsync(new DeviceBottomSheetContent
        {
            SignalRSubscribe = false
        });

        public async Task<string> TransformDevicePartyEventAsync(DevicePartyOutboundEvent outbound, Guid deviceId)
        {
            using var scope = scopeFactory.CreateDeviceScope(deviceId);
            var renderer = scope.ServiceProvider.GetRequiredService<IComponentStringRenderer>();

            return outbound switch
            {
                DeviceSelfLeftPartyOutboundEvent selfLeftEvent => await renderer.RenderComponentAsync(new AppendComponentLayout
                {
                    Components = new()
                    {
                        new DeviceBottomSheetContent
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
