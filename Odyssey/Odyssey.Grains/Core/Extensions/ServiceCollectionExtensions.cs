using Haondt.Orleans.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions;
using Odyssey.GrainInterfaces.Sessions.Models;
using Odyssey.GrainInterfaces.Sessions.Services;
using Orleans.Serialization;

namespace Odyssey.GrainInterfaces.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddOdysseyGrainServices(IConfiguration configuration)
            {
                services.AddHaondtOrleans(configuration);
                return services;
            }
        }
    }
}
