namespace Odyssey.Domain.Sessions.Events
{
    // TODO: probably don't need the inbound event as it will be done with HTTP POST. This is just illustrative
    [GenerateSerializer]
    public class PartyDisbandedInboundEvent : PartyInboundEvent
    {
        public const string Type = "PartyDisbanded";

    }

    [GenerateSerializer]
    public class PartyDisbandedOutboundEvent : PartyOutboundEvent
    {
        [Id(0)]
        public required string PartyId { get; set; }
    }
}
