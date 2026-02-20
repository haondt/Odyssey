using Microsoft.Extensions.DependencyInjection;
using Odyssey.Games.Domain.DebugGame.Extensions;

namespace Odyssey.Games.Domain.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOdysseyGames(this IServiceCollection services)
        {
            services.AddDebugGameServices();
            return services;
        }

    }
}
