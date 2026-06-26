namespace Odyssey.Client.Core.Services
{
    public class NoOpDeveloperDataSeeder : IDeveloperDataSeeder
    {
        public Task SeedAsync() => Task.CompletedTask;
    }
}
