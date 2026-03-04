using Haondt.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.Client.Authentication.Services;
using Odyssey.Client.Core.Services;
using Odyssey.UI.Host.Services;

namespace Odyssey.UI.Host.Hubs
{

    [Authorize]
    public abstract class HostHub<TInbound, TOutbound>(ISignalRConnectionRegistry<IHostSignalRConnectionBridge<TInbound>> registry) : Hub<IHostHubReceiver<TOutbound>>, IHostHubSender<TInbound>
    {
        public abstract IHostSignalRConnectionBridge<TInbound> CreateBridge(IServiceProvider serviceProvider, string userId);

        private async Task<Result<(string, HttpContext)>> GetAuthenticatedUserIdAsync()
        {
            if (Context.GetHttpContext() is not { } context)
                return new();

            var sessionService = context.RequestServices.GetRequiredService<ISessionService>();
            if (!sessionService.IsAuthenticated)
                return new();

            var userId = await sessionService.GetUserIdAsync();

            return new((userId, context));
        }


        public override async Task OnConnectedAsync()

        {
            if (await GetAuthenticatedUserIdAsync() is not { IsSuccessful: true, Value: var (userId, context) })
            {
                Context.Abort();
                return;
            }

            var bridge = CreateBridge(context.RequestServices, userId);

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

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (registry.Unregister(Context.ConnectionId).TryGetValue(out var bridge))
                await bridge.OnDisconnectedAsync();

            await base.OnDisconnectedAsync(exception);
        }

        public Task SendPartyEvent(TInbound body)
        {
            if (!registry.TryGetValue(Context.ConnectionId, out var connection))
                return Task.CompletedTask;

            return connection.SendPartyEvent(body);
        }
    }
}
