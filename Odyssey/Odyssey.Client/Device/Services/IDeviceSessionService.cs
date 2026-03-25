namespace Odyssey.Client.Device.Services
{
    public interface IDeviceSessionService
    {
        Guid DeviceId { get; }
        bool IsAuthenticated { get; }
    }
}
