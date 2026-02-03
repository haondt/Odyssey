using Microsoft.AspNetCore.Builder;

namespace Odyssey.UI.Core.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication UseOdysseyUI(this WebApplication app)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/static"
            });
            return app;
        }
    }
}
