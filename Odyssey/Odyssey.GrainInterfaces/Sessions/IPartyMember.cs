using Orleans.Concurrency;

namespace Odyssey.GrainInterfaces.Sessions
{
    public interface IPartyMember
    {
        [OneWay] // TODO: see if the runtime will log an uncaught error here
        Task NotifyPartyDisbandedAsync(string partyId);
    }
}
