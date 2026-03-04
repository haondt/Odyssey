using Odyssey.Domain.Core.Services;
using Odyssey.Domain.Sessions.Events;

namespace Odyssey.Domain.Host.Services
{
    public interface IHostEventTransformer<TInbound, TOutbound> : IEventTransformer
    {
        PartyInboundEvent TransformPartyEvent(TInbound inbound, string connectionId);
        Task<TOutbound> TransformPartyEventAsync(PartyOutboundEvent outbound, string userId);
    }
}
