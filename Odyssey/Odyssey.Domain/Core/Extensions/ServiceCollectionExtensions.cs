using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.Domain.Core.Services;

namespace Odyssey.Domain.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOdysseyDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(typeof(ICachedDataService<>), typeof(CachedDataService<>));
            services.AddSingleton<IGameRegistry, GameRegistry>();
            services.AddSingleton<IBoardService, BoardService>();

            return services;
        }
    }
}
