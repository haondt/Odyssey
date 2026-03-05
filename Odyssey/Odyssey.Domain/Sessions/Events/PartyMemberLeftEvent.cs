namespace Odyssey.Domain.Sessions.Events
{
    // TODO: probably don't need the inbound event as it will be done with HTTP POST. This is just illustrative
    [GenerateSerializer]
    public class PartyMemberLeftInboundEvent : PartyInboundEvent
    {
        [Id(0)]
        public required string MemberId { get; set; }
        public const string Type = "PartyMemberLeft";

    }
    [GenerateSerializer]
    public class PartyMemberLeftOutboundEvent : PartyOutboundEvent
    {
    }
}
