using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.Client.Authentication.Models;
using Odyssey.Client.Device.Models;
using Odyssey.Client.Display.Models;

namespace Odyssey.Client.Authentication.Services
{
    public class SignalRScopeFactory(IServiceScopeFactory scopeFactory) : ISignalRScopeFactory
    {
        public IServiceScope CreateScope(string userId)
        {
            var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<HostSessionContext>();
            context.IsAuthenticated = true;
            context.UserId = userId;

            var navigationManager = scope.ServiceProvider.GetRequiredService<NavigationManager>();
            if (navigationManager is IHostEnvironmentNavigationManager nm)
                nm.Initialize("http://localhost/", "http://localhost/");

            return scope;
        }

        public IServiceScope CreateDisplayScope(Guid displayId)
        {
            var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DisplaySessionContext>();
            context.DisplayId = displayId;

            var navigationManager = scope.ServiceProvider.GetRequiredService<NavigationManager>();
            if (navigationManager is IHostEnvironmentNavigationManager nm)
                nm.Initialize("http://localhost/", "http://localhost/");

            return scope;
        }

        public IServiceScope CreateDeviceScope(Guid deviceId)
        {
            var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DeviceSessionContext>();
            context.DeviceId = deviceId;

            var navigationManager = scope.ServiceProvider.GetRequiredService<NavigationManager>();
            if (navigationManager is IHostEnvironmentNavigationManager nm)
                nm.Initialize("http://localhost/", "http://localhost/");

            return scope;
        }
    }
}
