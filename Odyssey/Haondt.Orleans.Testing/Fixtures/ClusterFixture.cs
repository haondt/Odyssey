using Orleans.TestingHost;

namespace Haondt.Orleans.Testing.Fixtures
{
    public sealed class ClusterFixture<TSiloConfigurator, TClusterConfigurator> : IDisposable
        where TSiloConfigurator : ISiloConfigurator, new()
        where TClusterConfigurator : IClientBuilderConfigurator, new()
    {
        public TestCluster Cluster { get; private set; }

        public ClusterFixture()
        {
            Cluster = new TestClusterBuilder(1)
                .AddSiloBuilderConfigurator<TSiloConfigurator>()
                .AddClientBuilderConfigurator<TClusterConfigurator>()
                .Build();

            Cluster.Deploy();
        }

        public void Dispose() => Cluster.StopAllSilos();
    }

    public sealed class ClusterFixture<TSiloConfigurator> : IDisposable
        where TSiloConfigurator : ISiloConfigurator, new()
    {

        public TestCluster Cluster { get; private set; }

        public ClusterFixture()
        {
            Cluster = new TestClusterBuilder(1)
                .AddSiloBuilderConfigurator<TSiloConfigurator>()
                .Build();

            Cluster.Deploy();
        }

        public void Dispose() => Cluster.StopAllSilos();
    }
}
