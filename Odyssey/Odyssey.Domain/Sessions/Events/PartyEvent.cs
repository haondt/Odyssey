using Odyssey.Domain.Core.Events;

namespace Odyssey.Domain.Sessions.Events
{
    [GenerateSerializer]
    public abstract class PartyInboundEvent : SignalRInboundEvent
    {
    }
    [GenerateSerializer]
    public abstract class PartyOutboundEvent : SignalROutboundEvent
    {
    }
}
