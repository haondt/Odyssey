using Haondt.Orleans.Testing.Fixtures;
using Microsoft.Extensions.Configuration;
using Odyssey.Domain.Core.Extensions;
using Odyssey.GrainInterfaces.Core.Extensions;
using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.Persistence.Extensions;
using Orleans.TestingHost;

namespace Odyssey.IntegrationTests.Fixtures
{
    [CollectionDefinition(Name)]
    public class IntegrationTestsCollection : ICollectionFixture<IntegrationTestsWebApplicationFactory>
    {
        public const string Name = nameof(IntegrationTestsCollection);
    }
}
