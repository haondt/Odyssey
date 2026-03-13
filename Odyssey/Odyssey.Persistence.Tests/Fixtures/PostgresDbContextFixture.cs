namespace Odyssey.Persistence.Tests.Fixtures
{
    public class PostgresDbContextFixture : IAsyncLifetime
    {
        public PostgresApplicationDbContextFactory Factory { get; private set; } = default!;

        public async Task InitializeAsync()
        {
            Factory = await PostgresApplicationDbContextFactory.CreateAsync();
        }

        public async Task DisposeAsync() => await Factory.DisposeAsync();
    }
}
