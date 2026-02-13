using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Odyssey.Persistence.Models;

namespace Odyssey.Persistence.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static async Task PerformDatabaseMigrationsAsync(this IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
        {
            using var scope = serviceProvider.CreateScope();
            var persistenceSettings = scope.ServiceProvider.GetRequiredService<IOptions<PersistenceSettings>>();

            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            if (persistenceSettings.Value.DatabaseSettings.DropDatabaseOnStartup)
                await db.Database.EnsureDeletedAsync(cancellationToken);

            switch (persistenceSettings.Value.DatabaseSettings.Driver)
            {
                case DatabaseDriver.Postgres:
                    await db.Database.MigrateAsync(cancellationToken);
                    break;
                case DatabaseDriver.Memory:
                default:
                    await db.Database.EnsureCreatedAsync(cancellationToken);
                    break;
            }
        }
    }
}
