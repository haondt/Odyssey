using Haondt.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Npgsql;
using Odyssey.Domain.Core.Models;
using Odyssey.Persistence.Models;
using Odyssey.Silo.Core.Models;
using Odyssey.Silo.Core.Services;
using Orleans.Serialization;

namespace Odyssey.Silo.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        extension(IServiceCollection services)
        {

            public IServiceCollection AddOdysseySiloServices(IConfiguration configuration)
            {
                services.Configure<InitialServerSettings>(configuration.GetSection(nameof(InitialServerSettings)));
                services.ConfigureJsonSerializationOptions(configuration);

                // core
                services.AddSingleton<ISiloStartupParticipant, InitialServerSettingsApplicatorStartupParticipant>();
                return services;
            }

            public IServiceCollection AddGrainStorage(IConfiguration configuration, string storageName)
            {
                var persistenceSettings = configuration.GetSection<PersistenceSettings>();
                switch (persistenceSettings.DatabaseSettings.Driver)
                {

                    case DatabaseDriver.Memory:
                        break;
                    case DatabaseDriver.Postgres:
                        services.AddAdoNetGrainStorage(storageName, options =>
                        {
                            options.Invariant = AdoNetInvariants.InvariantNamePostgreSql;
                            options.ConnectionString = new NpgsqlConnectionStringBuilder
                            {
                                Host = persistenceSettings.DatabaseSettings.Postgres!.Host,
                                Database = persistenceSettings.DatabaseSettings.Postgres!.Database,
                                Username = persistenceSettings.DatabaseSettings.Postgres!.Username,
                                Password = persistenceSettings.DatabaseSettings.Postgres!.Password,
                                Port = persistenceSettings.DatabaseSettings.Postgres!.Port
                            }.ToString();
                        });
                        break;
                    default:
                        throw new ArgumentException($"Unknown database driver {persistenceSettings.DatabaseSettings.Driver}.");
                }

                return services;

            }

            public IServiceCollection ConfigureJsonSerializationOptions(IConfiguration configuration)
            {
                services.Configure<OrleansJsonSerializerOptions>(options =>
                {
                    options.JsonSerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
                    options.JsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    options.JsonSerializerSettings.Formatting = Formatting.None;
                    options.JsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.JsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver()
                    {
                        NamingStrategy = new CamelCaseNamingStrategy
                        {
                            ProcessDictionaryKeys = false
                        }
                    };
                });
                return services;
            }
        }
    }
}
