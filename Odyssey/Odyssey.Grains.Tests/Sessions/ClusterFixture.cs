using Orleans.TestingHost;

namespace Odyssey.Grains.Tests.Sessions
{
    public sealed class ClusterFixture<TSiloConfigurator, TClusterConfigurator> : IDisposable
        where TSiloConfigurator : ISiloConfigurator, new()
        where TClusterConfigurator : IClientBuilderConfigurator, new()
    {
        public TestCluster Cluster { get; } = new TestClusterBuilder(1)
            .AddSiloBuilderConfigurator<TSiloConfigurator>()
            .AddClientBuilderConfigurator<TClusterConfigurator>()
            .Build();

        public ClusterFixture() => Cluster.Deploy();
        public void Dispose() => Cluster.StopAllSilos();
    }

}
