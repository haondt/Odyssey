using Haondt.Core.Extensions;
using Microsoft.Data.Sqlite;
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

            switch (persistenceSettings.Driver)
            {
                case StorageDrivers.Memory:
                    services.AddDbContext<ApplicationDbContext>(o =>
                        o.UseInMemoryDatabase("Odyssey"));
                    break;

                case StorageDrivers.Sqlite:
                    var sqliteConnection = new SqliteConnectionStringBuilder
                    {
                        DataSource = Path.Join(persistenceSettings.FileDataPath, persistenceSettings.Sqlite!.FilePath)
                    }.ToString();

                    services.AddDbContext<SqliteApplicationDbContext>(o =>
                        o.UseSqlite(sqliteConnection));
                    services.AddScoped<ApplicationDbContext>(sp =>
                        sp.GetRequiredService<SqliteApplicationDbContext>());
                    break;

                case StorageDrivers.Postgres:
                    var pgConnection = new NpgsqlConnectionStringBuilder
                    {
                        Port = persistenceSettings.Postgres!.Port,
                        Host = persistenceSettings.Postgres!.Host,
                        Username = persistenceSettings.Postgres!.Username,
                        Password = persistenceSettings.Postgres!.Password,
                        Database = persistenceSettings.Postgres!.Database,
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
