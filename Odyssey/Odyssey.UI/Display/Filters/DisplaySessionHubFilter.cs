using Haondt.Core.Models;
using Haondt.Web.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.Client.Display.Models;
using System.Reflection;

namespace Odyssey.UI.Display.Filters
{
    public class DisplaySessionHubFilter : IHubFilter
    {
        private Optional<(HttpContext HttpContext, DisplaySessionContext DisplaySession)> TryGetSessionContext(HubLifetimeContext context)
        {
            if (context.Context.GetHttpContext() is not { } httpContext)
                return new();
            var sessionContext = httpContext.RequestServices.GetRequiredService<DisplaySessionContext>();
            return (httpContext, sessionContext);
        }

        public Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
        {
            if (context.Hub.GetType().GetCustomAttributes<DisplaySessionAttribute>().Any())
                if (TryGetSessionContext(context) is { HasValue: true, Value: var (httpContext, displaySession) })
                    if (DisplaySessionAttribute.TryGetDisplayId(httpContext.Request.AsRequestData(), out var displayId))
                        displaySession.DisplayId = displayId;

            return next(context);
        }
    }
}
