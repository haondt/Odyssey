using Haondt.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.Client.Display.Services;
using Odyssey.UI.Display.Filters;

namespace Odyssey.UI.Host.Hubs
{
    [DisplaySession]
    public class DisplayHub : Hub
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

            Console.WriteLine($"Hello {displayId}!");

            await base.OnConnectedAsync();
        }
    }
}
