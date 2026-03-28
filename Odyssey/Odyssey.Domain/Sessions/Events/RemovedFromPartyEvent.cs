namespace Odyssey.Domain.Sessions.Events
{
    [GenerateSerializer]
    public class RemovedFromPartyOutboundEvent : PartyOutboundEvent
    {
        [Id(0)]
        public required string PartyId { get; set; }
    }
}
