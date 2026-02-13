using Haondt.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Odyssey.Domain.Core.Models;
using Odyssey.Persistence.Models;
using Orleans.Configuration;

namespace Odyssey.Client.Core.Extensions
{
    public static class ClientBuilderExtensions
    {
        extension(IClientBuilder client)
        {

            public IClientBuilder ConfigureCluster(IConfiguration configuration)
            {
                var clusterSettings = configuration.GetRequiredSection<ClusterSettings>();
                if (clusterSettings.UseLocalhostClustering)
                    client.UseLocalhostClustering();
                else
                {
                    var persistenceSettings = configuration.GetSection<PersistenceSettings>();
                    switch (persistenceSettings.DatabaseSettings.Driver)
                    {

                        case DatabaseDriver.Memory:
                            client.UseLocalhostClustering();
                            break;
                        case DatabaseDriver.Postgres:
                            client.UseAdoNetClustering(options =>
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

                }
                client.Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = clusterSettings.ClusterId;
                    options.ServiceId = clusterSettings.ServiceId;
                });


                return client;
            }
        }
    }
}
