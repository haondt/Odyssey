using Microsoft.Extensions.DependencyInjection;
using Odyssey.Games.Domain.Core.Services;
using Odyssey.Games.Domain.DebugGame.Extensions;

namespace Odyssey.Games.Domain.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOdysseyGames(this IServiceCollection services)
        {
            services.AddDebugGame();
            services.AddSingleton<IClock, Clock>();
            return services;
        }

    }
}
