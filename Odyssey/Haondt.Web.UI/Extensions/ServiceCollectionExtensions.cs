using Haondt.Web.Services;
using Haondt.Web.UI.Filters;
using Haondt.Web.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Haondt.Web.UI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHaondtUI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHaondtUIHeadEntries();
            services.AddScoped<ModelStateValidationFilter>();
            services.AddScoped<ValidationState>();
            services.AddScoped<IValidationStateReader>(sp => sp.GetRequiredService<ValidationState>());
            services.AddScoped<IValidationStateWriter>(sp => sp.GetRequiredService<ValidationState>());
            services.AddScoped<RenderContext>();
            services.AddScoped<IRenderContextAccessor>(sp => sp.GetRequiredService<RenderContext>());
            services.AddScoped<IRenderContextMutator>(sp => sp.GetRequiredService<RenderContext>());
            services.Configure<LucideIconOptions>(_ => { });
            services.AddMemoryCache();
            services.AddSingleton<ILucideIconService, LucideIconService>();
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

        /// <summary>
        /// This must go before loading in hyperscript!
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHaondtUIHyperscriptScripts(this IServiceCollection services)
        {
            services.AddScoped<IHeadEntryDescriptor>(_ => new ScriptDescriptor
            {
                Uri = "/static/haondt/Haondt.Web.UI/_hs/toast._hs",
                Type = "text/hyperscript"
            });
            services.AddScoped<IHeadEntryDescriptor>(_ => new ScriptDescriptor
            {
                Uri = "/static/haondt/Haondt.Web.UI/_hs/moreButton._hs",
                Type = "text/hyperscript"
            });
            services.AddScoped<IHeadEntryDescriptor>(_ => new ScriptDescriptor
            {
                Uri = "/static/haondt/Haondt.Web.UI/_hs/field._hs",
                Type = "text/hyperscript"
            });
            services.AddScoped<IHeadEntryDescriptor>(_ => new ScriptDescriptor
            {
                Uri = "/static/haondt/Haondt.Web.UI/_hs/fieldSuggest._hs",
                Type = "text/hyperscript"
            });

            return services;
        }
    }
}
