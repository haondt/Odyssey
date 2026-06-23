using FluentAssertions;
using Haondt.Orleans.Testing.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions;
using Odyssey.GrainInterfaces.Sessions.Models;
using Odyssey.Grains.Sessions.Models;
using Odyssey.Grains.Tests.Sessions.Grains;
using Orleans.Storage;

namespace Odyssey.Grains.Tests.Sessions
{
    [Collection(SessionsCollection.Name)]
    public class PartyGrainTests(ClusterFixture<SessionsSiloConfigurator, SessionsClusterConfigurator> fixture)
    {
        [Fact]
        public async Task WillCleanUpOrphanedJoinCodesWhenJoiningADisbandedParty()
        {
            // create a party
            var hostId = Guid.NewGuid().ToString();
            var hostPartyGrainFactory = fixture.Cluster.Client.ServiceProvider.GetRequiredService<ICastedGrainFactory<string, IHostPartyGrain>>();
            var hostPartyGrain = hostPartyGrainFactory.GetGrain(hostId);

            // get the join code
            var joinCode = await hostPartyGrain.GetJoinCodeAsync();
            var joinCodeGrainFactory = fixture.Cluster.Client.ServiceProvider.GetRequiredService<IGrainFactory<string, IJoinCodeGrain>>();
            var joinCodeGrain = joinCodeGrainFactory.GetGrain(joinCode);


            // retrieve the party from the join code as a member
            var memberPartyGrain = await joinCodeGrain.GetMemberPartyAsync();
            memberPartyGrain.HasValue.Should().BeTrue();

            // externally delete the party
            await hostPartyGrain.DeactivateOnIdleAsync();

            var grainStorage = fixture.Cluster.GetSiloServiceProvider().GetRequiredKeyedService<IGrainStorage>(GrainConstants.GrainStorage);
            var partyGrainState = new GrainState<PartyGrainState>();
            await grainStorage.ReadStateAsync(nameof(PartyGrainState), hostPartyGrain.GetGrainId(), partyGrainState);
            await grainStorage.ClearStateAsync(nameof(PartyGrainState), hostPartyGrain.GetGrainId(), partyGrainState);

            // create a new party
            var newJoinCode = await hostPartyGrain.GetJoinCodeAsync();
            newJoinCode.Should().NotBeEquivalentTo(joinCode);

            // old joinCodeGrain should still exist
            var ownership = await joinCodeGrain.CheckOwnershipAsync(hostId);
            ownership.Should().BeTrue();

            // try to join using the old join code
            var memberId = Guid.NewGuid();
            var memberGrain = fixture.Cluster.Client.GetGrain<ITestPartyMemberGrain>(memberId);
            var joinResult = await memberPartyGrain.Value!.JoinAsync(new(memberId, PartyMemberType.Display), memberGrain, joinCode);
            joinResult.Should().BeFalse();

            // old joinCodeGrain should be cleaned up
            ownership = await joinCodeGrain.CheckOwnershipAsync(hostId);
            ownership.Should().BeFalse();
            await joinCodeGrain.DeactivateOnIdleAsync();
            ownership = await joinCodeGrain.CheckOwnershipAsync(hostId);
            ownership.Should().BeFalse();

            // verify we can join with the new join code
            joinResult = await memberPartyGrain.Value!.JoinAsync(new(memberId, PartyMemberType.Display), memberGrain, newJoinCode);
            joinResult.Should().BeTrue();
        }
    }
}
