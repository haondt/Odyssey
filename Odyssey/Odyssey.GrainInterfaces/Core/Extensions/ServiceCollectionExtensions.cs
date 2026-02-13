using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.GrainInterfaces.Core.Services;

namespace Odyssey.GrainInterfaces.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddOdysseyGrainInterfacesServices(IConfiguration configuration)
            {
                services.AddOdysseyGrainFactories();
                return services;
            }

            public IServiceCollection AddOdysseyGrainFactories()
            {
                // core
                services.AddSingleton(typeof(IDataStorageGrainFactory<>), typeof(DataStorageGrainFactory<>));
                services.AddSingleton(typeof(IDataStorageCacheGrainFactory<>), typeof(DataStorageCacheGrainFactory<>));
                return services;
            }
        }
    }
}
