using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Odyssey.Persistence.Tests
{
    public class PostgresApplicationDbContextFactory(
        DbContextOptions<PostgresApplicationDbContext> options,
        PostgreSqlContainer postgres) : IDbContextFactory<ApplicationDbContext>, IAsyncDisposable
    {
        public static async Task<PostgresApplicationDbContextFactory> CreateAsync()
        {
            var postgres = new PostgreSqlBuilder("postgres:18-alpine")
              .WithDatabase("haondt")
              .WithUsername("postgres")
              .WithPassword("postgres")
              .Build();
            await postgres.StartAsync();
            var options = new DbContextOptionsBuilder<PostgresApplicationDbContext>()
                .UseNpgsql(postgres.GetConnectionString())
                .Options;
            var factory = new PostgresApplicationDbContextFactory(options, postgres);

            await using var ctx = factory.CreateDbContext();
            await ctx.Database.MigrateAsync();
            return factory;
        }

        public ApplicationDbContext CreateDbContext() => new PostgresApplicationDbContext(options);

        public ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            return postgres.DisposeAsync();
        }
    }
}
