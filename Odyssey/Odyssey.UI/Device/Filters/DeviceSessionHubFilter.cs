using System.Reflection;
using Haondt.Core.Models;
using Haondt.Web.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.Client.Device.Models;

namespace Odyssey.UI.Device.Filters
{
    public class DeviceSessionHubFilter : IHubFilter
    {
        private Optional<(HttpContext HttpContext, DeviceSessionContext DeviceSession)> TryGetSessionContext(HubLifetimeContext context)
        {
            if (context.Context.GetHttpContext() is not { } httpContext)
                return new();
            var sessionContext = httpContext.RequestServices.GetRequiredService<DeviceSessionContext>();
            return (httpContext, sessionContext);
        }

        public Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
        {
            if (context.Hub.GetType().GetCustomAttributes<DeviceSessionAttribute>().Any())
                if (TryGetSessionContext(context) is { HasValue: true, Value: var (httpContext, deviceSession) })
                    if (DeviceSessionAttribute.TryGetDeviceId(httpContext.Request.AsRequestData(), out var deviceId))
                        deviceSession.DeviceId = deviceId;
            return next(context);
        }
    }
}
