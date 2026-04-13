using Microsoft.Extensions.DependencyInjection;
using Odyssey.Games.Domain.DebugGame.Models;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions;

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
