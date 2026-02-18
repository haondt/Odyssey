using Haondt.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Odyssey.Persistence.Models;
using Odyssey.Persistence.Services;


namespace Odyssey.Persistence.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static IServiceCollection AddOdysseyPersistenceServicesCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PersistenceSettings>(configuration.GetSection(nameof(PersistenceSettings)));
            var persistenceSettings = configuration.GetSection<PersistenceSettings>();
            switch (persistenceSettings.DatabaseSettings.Driver)
            {
                case DatabaseDriver.Memory:
                    services.AddDbContextFactory<MemoryApplicationDbContext>(o => o.UseInMemoryDatabase(GetInMemoryDatabaseName(persistenceSettings)));
                    services.AddSingleton<IDbContextFactory<ApplicationDbContext>, DbContextFactoryWrapper<MemoryApplicationDbContext, ApplicationDbContext>>();
                    break;
                case DatabaseDriver.Postgres:
                    services.AddDbContextFactory<PostgresApplicationDbContext>(o => o.UseNpgsql(GetPostgresConnectionString(persistenceSettings)));
                    services.AddSingleton<IDbContextFactory<ApplicationDbContext>, DbContextFactoryWrapper<PostgresApplicationDbContext, ApplicationDbContext>>();
                    break;
            }

            return services;
        }

        private static string GetPostgresConnectionString(PersistenceSettings persistenceSettings)
            => new NpgsqlConnectionStringBuilder
            {
                Port = persistenceSettings.DatabaseSettings.Postgres!.Port,
                Host = persistenceSettings.DatabaseSettings.Postgres!.Host,
                Username = persistenceSettings.DatabaseSettings.Postgres!.Username,
                Password = persistenceSettings.DatabaseSettings.Postgres!.Password,
                Database = persistenceSettings.DatabaseSettings.Postgres!.Database,
            }.ToString();

        private static string GetInMemoryDatabaseName(PersistenceSettings persistenceSettings) => "default";

        public static IServiceCollection AddOdysseyPersistenceClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOdysseyPersistenceServicesCore(configuration);
            services.AddScoped(sp => sp.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());

            return services;
        }

        public static IServiceCollection AddOdysseyPersistenceServerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOdysseyPersistenceServicesCore(configuration);

            return services;
        }
    }
}
