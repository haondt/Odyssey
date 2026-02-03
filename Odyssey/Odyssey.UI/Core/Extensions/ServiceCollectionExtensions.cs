using Haondt.Web.Core.Services;
using Haondt.Web.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.UI.Core.Middlewares;
using Odyssey.UI.Core.Services;
using ComponentFactory = Odyssey.UI.Core.Services.ComponentFactory;

namespace Odyssey.UI.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOdysseyUI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ILayoutComponentFactory, LayoutComponentFactory>();
            services.AddOdysseyHeadEntries();
            services.AddSingleton<IComponentFactory, ComponentFactory>();

            //services.AddSingleton<IExceptionActionResultFactory, ToastExceptionActionResultFactory>();
            services.AddScoped<ModelStateValidationFilter>();
            //services.AddSingleton<ILucideIconService, LucideIconService>();

            return services;
        }

        public static IServiceCollection AddOdysseyHeadEntries(this IServiceCollection services)
        {
            var assembly = typeof(ServiceCollectionExtensions).Assembly;
            var assemblyPrefix = assembly.GetName().Name;

            services.AddScoped<IHeadEntryDescriptor>(sp => new LinkDescriptor
            {
                Uri = "/static/shared/logo.svg",
                Relationship = "icon",
                Type = "image/svg+xml"
            });
            services.AddScoped<IHeadEntryDescriptor>(_ => new StyleSheetDescriptor
            {
                Uri = "/static/shared/css/style.css",
            });
            //services.AddScoped<IHeadEntryDescriptor>(sp => new ScriptDescriptor
            //{
            //    Uri = "/static/shared/vendored/htmx-ext-loading-states/loading-states.js"
            //});
            services.AddScoped<IHeadEntryDescriptor>(_ => new StyleSheetDescriptor
            {
                Uri = "/static/Odyssey.styles.css",
            });
            services.AddScoped<IHeadEntryDescriptor>(_ => new TitleDescriptor
            {
                Title = "Odyssey",
            });
            services.AddScoped<IHeadEntryDescriptor>(_ => new MetaDescriptor
            {
                Name = "htmx-config",
                Content = @"{
                    ""responseHandling"": [
                        { ""code"": ""204"", ""swap"": false },
                        { ""code"": "".*"", ""swap"": true }
                    ],
                    ""scrollIntoViewOnBoost"": false
                }",
            });

            // Add Inter font
            services.AddScoped<IHeadEntryDescriptor>(_ => new LinkDescriptor
            {
                Relationship = "preconnect",
                Uri = "https://fonts.googleapis.com"
            });
            services.AddScoped<IHeadEntryDescriptor>(_ => new LinkDescriptor
            {
                Relationship = "preconnect",
                Uri = "https://fonts.gstatic.com",
                CrossOrigin = new()
            });
            services.AddScoped<IHeadEntryDescriptor>(_ => new LinkDescriptor
            {
                Relationship = "stylesheet",
                Uri = "https://fonts.googleapis.com/css2?family=Inter:ital,opsz,wght@0,14..32,100..900;1,14..32,100..900&display=swap"
            });

            // PWA Meta Tags
            //services.AddScoped<IHeadEntryDescriptor>(_ => new MetaDescriptor
            //{
            //    Name = "theme-color",
            //    Content = "#181616"
            //});
            // PWA Manifest
            //services.AddScoped<IHeadEntryDescriptor>(_ => new LinkDescriptor
            //{
            //    Uri = "/static/shared/manifest.json",
            //    Relationship = "manifest"
            //});

            // Service Worker Registration Script
            //services.AddScoped<IHeadEntryDescriptor>(_ => new ScriptDescriptor
            //{
            //    Uri = "/static/shared/js/pwa-register.js"
            //});

            return services;
        }
    }
}
