namespace Odyssey.Domain.Core.Events
{
    [GenerateSerializer]
    public abstract class SignalRInboundEvent
    {
        [Id(0)]
        public required string OriginConnectionId { get; set; }
    }
}