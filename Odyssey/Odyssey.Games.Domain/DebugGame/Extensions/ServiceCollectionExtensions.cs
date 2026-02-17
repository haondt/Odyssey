using Microsoft.Extensions.DependencyInjection;
using Odyssey.Domain.Core.Models;
using Odyssey.Games.Domain.DebugGame.Services;

namespace Odyssey.Games.Domain.DebugGame.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDebugGame(this IServiceCollection services)
        {
            services.AddSingleton<IGame, DebugGameGame>();
            return services;
        }

    }
}
