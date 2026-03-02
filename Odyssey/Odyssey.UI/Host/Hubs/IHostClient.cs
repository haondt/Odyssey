namespace Odyssey.UI.Host.Hubs
{
    public interface IHostClient
    {
        // TODO: type that b
        Task ReceivePartyEvent(object payload);

        Task Counter(string count);

    }
}
