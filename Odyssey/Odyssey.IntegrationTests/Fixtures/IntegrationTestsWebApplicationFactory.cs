using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Odyssey.Domain.Core.Models;
using Odyssey.IntegrationTests.Fakes.Grains;
using Odyssey.IntegrationTests.Services.Http;
using Orleans.TestingHost.InMemoryTransport;
using Orleans.TestingHost.UnixSocketTransport;
namespace Odyssey.IntegrationTests.Fixtures
{
    public class IntegrationTestsWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            Program.ConfigureForTesting = b =>
            {
            };

            Program.AddOrleansClient = false;

            try
            {
                return base.CreateHost(builder);
            }
            finally
            {
                Program.ConfigureForTesting = null;
                Program.AddOrleansClient = true;
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddTransient<IntegrationTestsHttpClientFactory>();
                services.AddTransient<IGrainFactory, FakeGrainFactory>();
            });
        }
    }
}
