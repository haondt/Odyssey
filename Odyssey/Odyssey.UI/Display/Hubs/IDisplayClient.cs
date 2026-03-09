namespace Odyssey.UI.Display.Hubs
{
    public interface IDisplayHubReceiver<TOutbound>
    {
        Task ReceivePartyEvent(TOutbound body);
    }

    public interface IDisplayHubSender<TInbound>
    {
    }
}
