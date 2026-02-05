using Microsoft.AspNetCore.Builder;

namespace Odyssey.UI.Core.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication UseOdysseyUI(this WebApplication app)
        {
            return app;
        }
    }
}
