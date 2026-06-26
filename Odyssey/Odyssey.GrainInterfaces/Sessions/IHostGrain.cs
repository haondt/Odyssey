using Odyssey.GrainInterfaces.Core;
using Odyssey.GrainInterfaces.Sessions.Models;
using Orleans.Concurrency;

namespace Odyssey.GrainInterfaces.Sessions
{
    public interface IHostGrain : IGrain<string>, IGrainWithStringKey
    {
        [OneWay]
        Task NotifyPartyDisbandedAsync(string joinCode);

        [OneWay]
        Task NotifyPartyMemberJoinedAsync();
        [OneWay]
        Task NotifyPartyMemberLeftAsync();
        [OneWay]
        Task NotifyPartyMemberModifiedAsync();

        Task SetHostSettingsAsync(HostSettings settings);
        Task<HostSettings> GetHostSettingsAsync();
    }
}
