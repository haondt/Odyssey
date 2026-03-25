using Odyssey.Client.Device.Models;

namespace Odyssey.Client.Device.Services
{
    public class DeviceSessionService(DeviceSessionContext context) : IDeviceSessionService
    {
        public Guid DeviceId => context.DeviceId.Value;
        public bool IsAuthenticated => context.DeviceId.HasValue;
    }
}
