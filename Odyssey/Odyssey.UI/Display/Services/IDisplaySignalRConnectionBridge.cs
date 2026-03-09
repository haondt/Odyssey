using Odyssey.Domain.Core.Services;
using Odyssey.UI.Display.Hubs;

namespace Odyssey.UI.Display.Services
{
    public interface IDisplaySignalRConnectionBridge<TInbound> : ISignalRConnectionBridge, IDisplayHubSender<TInbound>
    {
        Guid DisplayId { get; }

    }
}
