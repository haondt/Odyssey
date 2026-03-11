using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.Domain.Core.Services;

namespace Odyssey.Domain.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOdysseyDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(typeof(ICachedDataRepository<>), typeof(CachedDataRepository<>));
            services.AddSingleton<IBoardMetadataRepository, BoardMetadataRepository>();
            services.AddSingleton<ISessionMetadataRepository, SessionMetadataRepository>();
            services.AddSingleton<IGrainLeaseService, GrainLeaseService>();
            services.AddTransient<IComponentStringRenderer, ComponentStringRenderer>();
            services.AddTransient<HtmlRenderer>();
            services.AddSingleton<IEventTransformerRegistry, EventTransformerRegistry>();

            return services;
        }
    }
}
