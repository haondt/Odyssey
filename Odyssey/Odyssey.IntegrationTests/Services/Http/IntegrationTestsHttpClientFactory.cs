using Odyssey.IntegrationTests.Fixtures;

namespace Odyssey.IntegrationTests.Services.Http
{
    public class IntegrationTestsHttpClientFactory
    {
        public HttpClientWrapper CreateClientWrapper(IntegrationTestsWebApplicationFactory factory)
        {
            var client = CreateClient(factory);
            return new(client);
        }
        public HttpClient CreateClient(IntegrationTestsWebApplicationFactory factory)
        {
            var httpClient = factory.CreateClient();
            return httpClient;
        }
    }
}
