using Haondt.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Odyssey.Client.Core.Services;
using Odyssey.Client.Display.Services;
using Odyssey.UI.Display.Filters;
using Odyssey.UI.Display.Services;
using Odyssey.UI.Host.Models;

namespace Odyssey.UI.Display.Hubs
{
    [DisplaySession]
    public class DisplayHub(
        ISignalRConnectionRegistry<IDisplaySignalRConnectionBridge<HtmxSignalRMessage>> registry,
        IServiceProvider serviceProvider,
        ILogger<DisplayHub> logger) : Hub<IDisplayHubReceiver<string>>, IDisplayHubSender<HtmxSignalRMessage>
    {
        private Result<(Guid, HttpContext)> GetDisplayId()
        {
            if (Context.GetHttpContext() is not { } context)
                return new();

            var sessionService = context.RequestServices.GetRequiredService<IDisplaySessionService>();
            if (!sessionService.IsAuthenticated)
                return new();

            return new((sessionService.DisplayId, context));
        }

        public override async Task OnConnectedAsync()
        {
            if (GetDisplayId() is not { IsSuccessful: true, Value: var (displayId, context) })
            {
                Context.Abort();
                return;
            }
            var bridge = ActivatorUtilities.CreateInstance<DisplaySignalRConnectionBridge<DisplayHub>>(serviceProvider, Context.ConnectionId, displayId);

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
                logger.LogDebug("Established connection bridge for {DisplayId}", displayId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (registry.Unregister(Context.ConnectionId).TryGetValue(out var bridge))
            {
                await bridge.OnDisconnectedAsync();
                if (logger.IsEnabled(LogLevel.Debug))
                    logger.LogDebug("Disconnected connection bridge for {DisplayId}", bridge.DisplayId);
            }

            await base.OnDisconnectedAsync(exception);
        }

    }
}
