using Odyssey.Domain.Core.Services;
using Odyssey.Domain.Display.Events;
using Odyssey.Domain.Sessions.Events;

namespace Odyssey.Domain.Display.Services
{
    public interface IDisplayEventTransformer<TInbound, TOutbound> : IEventTransformer
    {
        Task<TOutbound> TransformPartyEventAsync(PartyOutboundEvent outbound, Guid displayId);
        Task<TOutbound> TransformDisplayPartyEventAsync(DisplayPartyOutboundEvent outbound, Guid displayId);
    }
}
