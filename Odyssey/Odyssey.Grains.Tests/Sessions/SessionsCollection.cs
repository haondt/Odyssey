using Microsoft.Extensions.Configuration;
using Odyssey.Domain.Core.Extensions;
using Odyssey.GrainInterfaces.Core.Extensions;
using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.Persistence.Extensions;
using Orleans.TestingHost;

namespace Odyssey.Grains.Tests.Sessions
{
    [CollectionDefinition(Name)]
    public class SessionsCollection : ICollectionFixture<ClusterFixture<SessionsSiloConfigurator, SessionsClusterConfigurator>>
    {
        public const string Name = nameof(SessionsCollection);
    }

    public class SessionsSiloConfigurator : ISiloConfigurator
    {
        public void Configure(ISiloBuilder siloBuilder)
        {
            //var testConfigFile = Path.Combine(Environment.CurrentDirectory, "appsettings.Test.json");
            //if (File.Exists(testConfigFile))
            //    siloBuilder.Configuration.AddJsonFile(testConfigFile, optional: true, reloadOnChange: true);

            siloBuilder
                .AddMemoryGrainStorage(GrainConstants.GrainStorage)
                .AddMemoryGrainStorage(GrainConstants.SignalRStreams)
                .AddMemoryStreams(GrainConstants.SignalRStreams);

            siloBuilder.ConfigureServices(services =>
            {
                services
                    .AddOdysseyGrainInterfacesServices(siloBuilder.Configuration)
                    .AddOdysseyPersistenceServerServices(siloBuilder.Configuration)
                    .AddOdysseyDomainServices(siloBuilder.Configuration);
                //.AddOdysseySiloServices(context.Configuration)
            });
        }
    }
    public class SessionsClusterConfigurator : IClientBuilderConfigurator
    {
        public void Configure(IConfiguration configuration, IClientBuilder builder)
        {
            builder.Services
                .AddOdysseyGrainInterfacesServices(builder.Configuration);
            //.AddOdysseyPersistenceClientServices(builder.Configuration)
            //.AddOdysseyDomainServices(builder.Configuration)
            //.AddOdysseyClientServices(builder.Configuration);
            builder
                .AddMemoryStreams(GrainConstants.SignalRStreams);
        }
    }
}
