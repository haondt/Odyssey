using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions;

namespace Odyssey.GrainInterfaces.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddOdysseyGrainInterfacesServices(IConfiguration configuration)
            {
                services.AddSingleton<IClock, Clock>();
                services.AddOdysseyGrainFactories();
                return services;
            }

            public IServiceCollection AddOdysseyGrainFactories()
            {
                // core
                services.AddSingleton(typeof(IDataStorageGrainFactory<>), typeof(DataStorageGrainFactory<>));
                services.AddSingleton<IGrainLeaseGrainFactory, GrainLeaseGrainFactory>();
                services.AddSingleton<ICrockfordService, CrockfordService>();

                // sessions
                services.AddSingleton<IGrainFactory<string, IHostGrain>, StringKeyGrainFactory<IHostGrain>>();
                services.AddSingleton<IGrainFactory<string, IHostPartyGrain>, StringKeyGrainFactory<IHostPartyGrain>>();
                services.AddSingleton<IGrainFactory<string, IPartyGrain>, StringKeyGrainFactory<IPartyGrain>>();
                services.AddSingleton<IGrainFactory<string, IJoinCodeGrain>, StringKeyGrainFactory<IJoinCodeGrain>>();
                return services;
            }
        }
    }
}
