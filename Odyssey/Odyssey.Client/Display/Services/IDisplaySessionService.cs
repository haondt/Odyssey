namespace Odyssey.Client.Display.Services
{
    public interface IDisplaySessionService
    {
        Guid DisplayId { get; }
        bool IsAuthenticated { get; }
    }
}
