using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Sessions.Models;
using Odyssey.GrainInterfaces.Sessions.Reasons;
using Orleans.Concurrency;

namespace Odyssey.GrainInterfaces.Sessions
{
    public interface IPartyMemberGrain : IGrain
    {
        [OneWay]
        Task NotifyPartyDisbandedAsync(string joinCode);
        [OneWay]
        Task NotifyPartyMemberJoinedAsync();
        [OneWay]
        Task NotifyPartyMemberLeftAsync();
        [OneWay]
        Task NotifyPartyMemberModifiedAsync();
        Task<DetailedResult<IMemberPartyGrain, JoinPartyReason>> JoinPartyAsync(string joinCode);
        Task<DetailedResult<LeavePartyReason>> LeavePartyAsync(string joinCode);
        Task<PartyMemberProfile> GetMemberProfileAsync();
        Task<Optional<MemberPartyDetails>> GetMemberPartyAsync();
    }
}
