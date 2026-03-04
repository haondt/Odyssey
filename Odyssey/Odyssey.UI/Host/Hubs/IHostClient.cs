namespace Odyssey.UI.Host.Hubs
{
    public interface IHostHubReceiver<TOutbound>
    {
        Task ReceivePartyEvent(TOutbound body);
    }

    public interface IHostHubSender<TInbound>
    {
        Task SendPartyEvent(TInbound body);
    }
}
