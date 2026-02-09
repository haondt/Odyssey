using Haondt.Web.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Haondt.Web.UI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHaondtUI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHaondtUIHeadEntries();
            return services;
        }

        public static IServiceCollection AddHaondtUIHeadEntries(this IServiceCollection services)
        {
            services.AddScoped<IHeadEntryDescriptor>(_ => new StyleSheetDescriptor
            {
                Uri = "/static/haondt/Haondt.Web.UI/css/style.css",
            });
            //services.AddScoped<IHeadEntryDescriptor>(_ => new StyleSheetDescriptor
            //{
            //    Uri = "/static/haondt/Haondt.Web.UI/styles.css",
            //});

            return services;
        }
    }
}
