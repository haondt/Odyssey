using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Odyssey.Persistence.Models;

namespace Odyssey.Persistence.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void PerformDatabaseMigrations(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var persistenceSettings = scope.ServiceProvider.GetRequiredService<IOptions<PersistenceSettings>>();

            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            if (persistenceSettings.Value.DatabaseSettings.DropDatabaseOnStartup)
                db.Database.EnsureDeleted();

            switch (persistenceSettings.Value.DatabaseSettings.Driver)
            {
                case DatabaseDriver.Sqlite:
                case DatabaseDriver.Postgres:
                    db.Database.Migrate();
                    break;
                default:
                    db.Database.EnsureCreated();
                    break;
            }
        }
    }
}
