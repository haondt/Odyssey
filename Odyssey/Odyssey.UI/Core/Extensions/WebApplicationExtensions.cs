using Microsoft.AspNetCore.Builder;
using Odyssey.UI.Core.Models;
using Odyssey.UI.Device.Hubs;
using Odyssey.UI.Display.Hubs;
using Odyssey.UI.Host.Hubs;

namespace Odyssey.UI.Core.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication UseOdysseyUI(this WebApplication app)
        {
            app.MapHub<BrowserHostHub>(OdysseyRoutes.Hubs.Host.Browser.Index);
            app.MapHub<DisplayHub>(OdysseyRoutes.Hubs.Display.Browser.Index);
            app.MapHub<BrowserDeviceHub>(OdysseyRoutes.Hubs.Device.Browser.Index);
            return app;
        }
    }
}
