namespace Odyssey.Domain.Core.Services
{
    public interface ISignalRConnectionBridge
    {
        Task OnConnectedAsync();
        Task OnDisconnectedAsync();
    }
}