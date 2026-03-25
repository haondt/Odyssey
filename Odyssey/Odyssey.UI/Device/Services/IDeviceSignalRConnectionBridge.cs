using Odyssey.Domain.Core.Services;
using Odyssey.UI.Device.Hubs;

namespace Odyssey.UI.Device.Services
{
    public interface IDeviceSignalRConnectionBridge<TInbound> : ISignalRConnectionBridge, IDeviceHubSender<TInbound>
    {
        Guid DeviceId { get; }
    }
}
