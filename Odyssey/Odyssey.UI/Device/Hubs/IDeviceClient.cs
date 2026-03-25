namespace Odyssey.UI.Device.Hubs
{
    public interface IDeviceHubReceiver<TOutbound>
    {
        Task ReceivePartyEvent(TOutbound body);
    }

    public interface IDeviceHubSender<TInbound>
    {
    }
}
