using Haondt.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Odyssey.Persistence.Models;


namespace Odyssey.Persistence.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOdysseyPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PersistenceSettings>(configuration.GetSection(nameof(PersistenceSettings)));
            var persistenceSettings = configuration.GetSection<PersistenceSettings>();

            switch (persistenceSettings.DatabaseSettings.Driver)
            {

                case DatabaseDriver.Memory:
                    services.AddDbContext<MemoryApplicationDbContext>(o =>
                        o.UseInMemoryDatabase("default"));
                    services.AddScoped<ApplicationDbContext>(sp =>
                        sp.GetRequiredService<MemoryApplicationDbContext>());
                    break;

                case DatabaseDriver.Postgres:
                    var pgConnection = new NpgsqlConnectionStringBuilder
                    {
                        Port = persistenceSettings.DatabaseSettings.Postgres!.Port,
                        Host = persistenceSettings.DatabaseSettings.Postgres!.Host,
                        Username = persistenceSettings.DatabaseSettings.Postgres!.Username,
                        Password = persistenceSettings.DatabaseSettings.Postgres!.Password,
                        Database = persistenceSettings.DatabaseSettings.Postgres!.Database,
                    }.ToString();

                    services.AddDbContext<PostgresApplicationDbContext>(o =>
                        o.UseNpgsql(pgConnection));
                    services.AddScoped<ApplicationDbContext>(sp =>
                        sp.GetRequiredService<PostgresApplicationDbContext>());
                    break;
            }

            return services;
        }
    }
}
