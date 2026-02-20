using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.Games.Client.DebugGame.Extensions;

namespace Odyssey.Games.Client.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOdysseyGamesClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDebugGameClientServices();
            return services;
        }

    }
}
