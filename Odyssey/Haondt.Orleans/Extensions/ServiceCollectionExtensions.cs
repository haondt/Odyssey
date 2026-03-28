using Haondt.Orleans.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Haondt.Orleans.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHaondtOrleans(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRewindablePersistentStateFactory, RewindablePersistentStateFactory>();
            return services;
        }
    }
}
