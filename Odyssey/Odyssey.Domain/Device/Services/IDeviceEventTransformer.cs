using Odyssey.Domain.Core.Services;
using Odyssey.Domain.Device.Events;
using Odyssey.Domain.Sessions.Events;

namespace Odyssey.Domain.Device.Services
{
    public interface IDeviceEventTransformer<TInbound, TOutbound> : IEventTransformer
    {
        Task<TOutbound> TransformPartyEventAsync(PartyOutboundEvent outbound, Guid deviceId);
        Task<TOutbound> TransformDevicePartyEventAsync(DevicePartyOutboundEvent outbound, Guid deviceId);
    }
}
