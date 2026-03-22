using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Odyssey.Client.Core.Services;
using Odyssey.UI.Host.Models;
using Odyssey.UI.Host.Services;

namespace Odyssey.UI.Host.Hubs
{
    public class BrowserHostHub(ISignalRConnectionRegistry<IHostSignalRConnectionBridge<HtmxSignalRMessage>> registry, ILogger<BrowserHostHub> logger) : HostHub<HtmxSignalRMessage, string>(registry, logger)
    {
        public override IHostSignalRConnectionBridge<HtmxSignalRMessage> CreateBridge(IServiceProvider serviceProvider, string userId)
        {
            return ActivatorUtilities.CreateInstance<HostSignalRConnectionBridge<HtmxSignalRMessage, string, BrowserHostHub>>(serviceProvider, Context.ConnectionId, HostClientType.Browser, userId);
        }
    }
}
