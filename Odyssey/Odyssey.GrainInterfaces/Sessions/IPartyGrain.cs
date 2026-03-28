using Haondt.Core.Models;
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
        Task UpdateDisplayDataAsync(PartyMemberId memberId, HostDisplayData data, bool upsert = false);
        Task UpdateDeviceDataAsync(PartyMemberId memberId, HostDeviceData data, bool upsert = false);
        Task<HostDisplayData> GetDisplayDataAsync(PartyMemberId memberId);
        Task<HostDeviceData> GetDeviceDataAsync(PartyMemberId memberId);
    }

    public interface IMemberPartyGrain : ICommonPartyGrain
    {
        Task<bool> LeaveAsync(PartyMemberId memberId, Optional<string> joinCode = default);
        Task<bool> JoinAsync(PartyMemberId memberId, IPartyMemberGrain member, string joinCode);
        Task<MemberPartyDetails> GetPartyDetailsAsync(PartyMemberId requesterId, PartyMemberProfile requesterProfile);
    }

    public interface ICommonPartyGrain : IGrain
    {

    }
}
