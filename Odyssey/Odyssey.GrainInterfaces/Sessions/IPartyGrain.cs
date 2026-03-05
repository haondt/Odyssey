using Odyssey.GrainInterfaces.Core;
using Odyssey.GrainInterfaces.Sessions.Models;
using Odyssey.GrainInterfaces.Testing;

namespace Odyssey.GrainInterfaces.Sessions
{
    public interface IPartyGrain : IGrain<string>, IGrainWithStringKey, IHostPartyGrain, IMemberPartyGrain
    {
    }

    public interface IHostPartyGrain : ICommonPartyGrain, IDeactivatableGrain
    {
        Task<string> GetJoinCodeAsync();
        Task ResetPartyAsync();
        Task<HostPartyDetails> GetPartyDetailsAsync();
        Task SetHostDataAsync(HostPartyData data);
        Task<HostPartyData> GetHostDataAsync();
    }

    public interface IMemberPartyGrain : ICommonPartyGrain
    {
        Task LeaveAsync(IPartyMemberGrain member);
        Task<bool> JoinAsync(IPartyMemberGrain member, string joinCode);
        Task<MemberPartyDetails> GetPartyDetailsAsync(IPartyMemberGrain requester, PartyMemberProfile requesterProfile);
    }

    public interface ICommonPartyGrain : IGrain
    {

    }
}
