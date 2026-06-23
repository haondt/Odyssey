using FluentAssertions;
using Haondt.Orleans.Testing.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions.Models;
using Odyssey.Grains.Sessions.Models;
using Odyssey.IntegrationTests.Fixtures;
using Odyssey.IntegrationTests.Services.Http;
using Odyssey.UI.Authentication.Models.Authentication;
using Odyssey.UI.Core.Models;
using Orleans.Storage;
using Xunit.Abstractions;

namespace Odyssey.IntegrationTests
{
    [Collection(IntegrationTestsCollection.Name)]
    public class SessionTests
    {

        private readonly IntegrationTestsWebApplicationFactory _factory;
        private readonly IntegrationTestsHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private readonly HttpClientWrapper _httpClientWrapper;

        public SessionTests(
                IntegrationTestsWebApplicationFactory factory,
                ITestOutputHelper output)
        {
            _factory = factory;
            _httpClientFactory = _factory.Services.GetRequiredService<IntegrationTestsHttpClientFactory>();
            _httpClient = _httpClientFactory.CreateClient(_factory);
            _httpClientWrapper = _httpClientFactory.CreateClientWrapper(_factory);
        }

        [Fact]
        public async Task CanSignInAsDefaultAdminUser()
        {
            var response = await _httpClientWrapper.PostAsFormDataAsync(OdysseyRoutes.Auth.SignIn, new SignInModel
            {
                Username = "admin",
                Password = "P@ssword1234"
            });
            response.EnsureSuccessStatusCode();

            response.Headers.Should().ContainKey("HX-Redirect");
            response.Headers.GetValues("HX-Redirect").First().Should().Be("/roles");
        }
    }
}
