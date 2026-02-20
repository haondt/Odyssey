using Microsoft.Extensions.DependencyInjection;
using Odyssey.Client.Core.Services;
using Odyssey.Games.Client.DebugGame.Core.Services;

namespace Odyssey.Games.Client.DebugGame.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDebugGameClientServices(this IServiceCollection services)
        {
            services.AddSingleton<IClientGame, DebugGameClientGame>();
            return services;
        }

    }
}
