using Odyssey.GrainInterfaces.Core;
using Orleans.Concurrency;

namespace Odyssey.GrainInterfaces.Sessions
{
    public interface IHostGrain : IGrain<string>, IGrainWithStringKey
    {
        [OneWay]
        Task NotifyPartyDisbandedAsync(string partyId);

        [OneWay]
        Task NotifyPartyMemberJoinedAsync();
        [OneWay]
        Task NotifyPartyMemberLeftAsync();
    }
}