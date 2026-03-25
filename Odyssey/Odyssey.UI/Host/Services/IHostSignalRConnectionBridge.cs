using Odyssey.Domain.Core.Services;
using Odyssey.UI.Host.Hubs;

namespace Odyssey.UI.Host.Services
{
    public interface IHostSignalRConnectionBridge<TInbound> : ISignalRConnectionBridge, IHostHubSender<TInbound>
    {
        string UserId { get; }
    }
}
