using Odyssey.Persistence;

namespace Odyssey.Domain.Services;

public class DbSeeder(ApplicationDbContext dbContext) : IDbSeeder
{
    public async Task SeedAsync()
    {
    }
}
