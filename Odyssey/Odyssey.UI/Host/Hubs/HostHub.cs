using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.Client.Authentication.Services;
using Odyssey.Domain.Core.Services;
using Odyssey.UI.Host.Components;

namespace Odyssey.UI.Host.Hubs
{

    [Authorize]
    public class HostHub : Hub<IHostClient>
    {
        public static int count = 0;
        public override async Task OnConnectedAsync()
        {
            await TryLinkDeliveryGrainAsync();
            count += 1;

            var renderer = Context.GetHttpContext()!.RequestServices.GetRequiredService<IComponentStringRenderer>();
            await Clients.All.Counter(await renderer.RenderComponentAsync(new TestComponent { Count = count }));
            await base.OnConnectedAsync();
        }

        private async Task TryLinkDeliveryGrainAsync()
        {
            Console.WriteLine("linking..");
            if (Context.GetHttpContext() is not { } context)
                return;

            var sessionService = context.RequestServices.GetRequiredService<ISessionService>();
            if (!sessionService.IsAuthenticated)
                return;

            var userId = await sessionService.GetUserIdAsync();

            Console.WriteLine($"user {userId} connected.");

        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {

            await TryUnlinkDeliveryGrainAsync();
            count -= 1;
            var renderer = Context.GetHttpContext()!.RequestServices.GetRequiredService<IComponentStringRenderer>();
            await Clients.All.Counter(await renderer.RenderComponentAsync(new TestComponent { Count = count }));
            await base.OnDisconnectedAsync(exception);
        }

        private async Task TryUnlinkDeliveryGrainAsync()
        {
            Console.WriteLine("unlinking..");
            if (Context.GetHttpContext() is not { } context)
                return;

            var sessionService = context.RequestServices.GetRequiredService<ISessionService>();
            if (!sessionService.IsAuthenticated)
                return;

            var userId = await sessionService.GetUserIdAsync();

            Console.WriteLine($"user {userId} disconnected.");

        }

    }
}
