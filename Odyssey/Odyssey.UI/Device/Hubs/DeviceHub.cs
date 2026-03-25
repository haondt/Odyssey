using Haondt.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Odyssey.Client.Core.Services;
using Odyssey.Client.Device.Services;
using Odyssey.UI.Device.Filters;
using Odyssey.UI.Device.Services;
using Odyssey.UI.Host.Models;

namespace Odyssey.UI.Device.Hubs
{

    [DeviceSession]
    public abstract class DeviceHub<TInbound, TOutbound>(
        ISignalRConnectionRegistry<IDeviceSignalRConnectionBridge<TInbound>> registry,
        ILogger<DeviceHub<TInbound, TOutbound>> logger) : Hub<IDeviceHubReceiver<TOutbound>>, IDeviceHubSender<TInbound>
    {
        public abstract IDeviceSignalRConnectionBridge<TInbound> CreateBridge(IServiceProvider serviceProvider, Guid deviceId);

        private Result<(Guid, HttpContext)> GetDeviceId()
        {
            if (Context.GetHttpContext() is not { } context)
                return new();

            var sessionService = context.RequestServices.GetRequiredService<IDeviceSessionService>();
            if (!sessionService.IsAuthenticated)
                return new();

            return new((sessionService.DeviceId, context));
        }

        public override async Task OnConnectedAsync()
        {
            if (GetDeviceId() is not { IsSuccessful: true, Value: var (deviceId, context) })
            {
                Context.Abort();
                return;
            }
            var bridge = CreateBridge(context.RequestServices, deviceId);

            registry.Register(Context.ConnectionId, bridge);
            try
            {
                await bridge.OnConnectedAsync();
            }
            catch
            {
                registry.Unregister(Context.ConnectionId);
                throw;
            }

            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug("Established connection bridge for Device {DeviceId}", deviceId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (registry.Unregister(Context.ConnectionId).TryGetValue(out var bridge))
            {
                await bridge.OnDisconnectedAsync();
                if (logger.IsEnabled(LogLevel.Debug))
                    logger.LogDebug("Disconnected connection bridge for Device {DeviceId}", bridge.DeviceId);
            }

            await base.OnDisconnectedAsync(exception);
        }

    }
}
