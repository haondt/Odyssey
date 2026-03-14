using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Sessions;
using Odyssey.GrainInterfaces.Sessions.Models;
using Odyssey.GrainInterfaces.Sessions.Reasons;

namespace Odyssey.Grains.Tests.Sessions.Grains
{
    public class TestPartyMemberGrain : Grain, ITestPartyMemberGrain
    {
        public Task<Optional<MemberPartyDetails>> GetMemberPartyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PartyMemberProfile> GetMemberProfileAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DetailedResult<IMemberPartyGrain, JoinPartyReason>> JoinPartyAsync(string joinCode)
        {
            throw new NotImplementedException();
        }

        public Task<DetailedResult<LeavePartyReason>> LeavePartyAsync(string joinCode)
        {
            throw new NotImplementedException();
        }

        public Task NotifyPartyDisbandedAsync(string joinCode)
        {
            throw new NotImplementedException();
        }

        public Task NotifyPartyMemberJoinedAsync()
        {
            throw new NotImplementedException();
        }

        public Task NotifyPartyMemberLeftAsync()
        {
            throw new NotImplementedException();
        }
    }
}
