using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Odyssey.Client.Core.Services;
using Odyssey.UI.Core.Models;
using Odyssey.UI.Device.Models;
using Odyssey.UI.Device.Services;

namespace Odyssey.UI.Device.Hubs
{
    public class BrowserDeviceHub(ISignalRConnectionRegistry<IDeviceSignalRConnectionBridge<HtmxSignalRMessage>> registry, ILogger<BrowserDeviceHub> logger) : DeviceHub<HtmxSignalRMessage, string>(registry, logger)
    {
        public override IDeviceSignalRConnectionBridge<HtmxSignalRMessage> CreateBridge(IServiceProvider serviceProvider, Guid deviceId)
        {
            return ActivatorUtilities.CreateInstance<DeviceSignalRConnectionBridge<HtmxSignalRMessage, string, BrowserDeviceHub>>(serviceProvider, Context.ConnectionId, DeviceClientType.Browser, deviceId);
        }
    }
}
