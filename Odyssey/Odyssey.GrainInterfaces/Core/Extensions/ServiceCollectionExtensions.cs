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
            public IServiceCollection AddOdysseyGrainInterfacesServices(IConfiguration configuration)
            {
                services.AddSingleton<IClock, Clock>();
                services.AddOdysseyGrainFactories();
                services.Configure<ExceptionSerializationOptions>(options =>
                {
                    options.SupportedNamespacePrefixes.Add("Newtonsoft.Json");
                    options.SupportedNamespacePrefixes.Add("Odyssey.GrainInterfaces.Sessions.Exceptions");
                });

                // sessions
                services.Configure<SessionSettings>(configuration.GetSection(nameof(SessionSettings)));

                return services;
            }

            public IServiceCollection AddOdysseyGrainFactories()
            {
                // core
                services.AddSingleton(typeof(IDataStorageGrainFactory<>), typeof(DataStorageGrainFactory<>));
                services.AddSingleton<IGrainLeaseGrainFactory, GrainLeaseGrainFactory>();
                services.AddSingleton<ICrockfordService, CrockfordService>();
                services.AddSingleton<INameGenerator, NameGenerator>();

                // sessions
                services.AddSingleton<IGrainFactory<string, IHostGrain>, StringKeyGrainFactory<IHostGrain>>();
                services.AddSingleton<IGrainFactory<string, IPartyGrain>, StringKeyGrainFactory<IPartyGrain>>();
                services.AddSingleton<ICastedGrainFactory<string, IHostPartyGrain>, CastedGrainFactory<string, IPartyGrain, IHostPartyGrain>>();
                services.AddSingleton<ICastedGrainFactory<string, IMemberPartyGrain>, CastedGrainFactory<string, IPartyGrain, IMemberPartyGrain>>();
                services.AddSingleton<IGrainFactory<string, IJoinCodeGrain>, StringKeyGrainFactory<IJoinCodeGrain>>();
                services.AddSingleton<IGrainFactory<Guid, IDisplayGrain>, GuidKeyGrainFactory<IDisplayGrain>>();
                services.AddSingleton<IGrainFactory<Guid, IDeviceGrain>, GuidKeyGrainFactory<IDeviceGrain>>();
                services.AddSingleton(typeof(ISessionGrainFactory<,>), typeof(SessionGrainFactory<,>));
                return services;
            }
        }
    }
}
