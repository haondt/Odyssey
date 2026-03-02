using Microsoft.AspNetCore.Builder;
using Odyssey.UI.Core.Models;
using Odyssey.UI.Host.Hubs;

namespace Odyssey.UI.Core.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication UseOdysseyUI(this WebApplication app)
        {
            app.MapHub<HostHub>(OdysseyRoutes.Hubs.Host.Index);
            return app;
        }
    }
}
