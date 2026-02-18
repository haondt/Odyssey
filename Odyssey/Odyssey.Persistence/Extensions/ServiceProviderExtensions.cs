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
            var persistenceSettings = serviceProvider.GetRequiredService<IOptions<PersistenceSettings>>();

            using var db = await serviceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContextAsync(cancellationToken);
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
