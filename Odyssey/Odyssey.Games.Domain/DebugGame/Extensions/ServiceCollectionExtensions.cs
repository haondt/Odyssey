using Microsoft.Extensions.DependencyInjection;

namespace Odyssey.Games.Domain.DebugGame.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDebugGameServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
