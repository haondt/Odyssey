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
            services.AddSingleton<IClock, Clock>();
            services.AddSingleton<IBoardMetadataRepository, BoardMetadataService>();

            return services;
        }
    }
}
