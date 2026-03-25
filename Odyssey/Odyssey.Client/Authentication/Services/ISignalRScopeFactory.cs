using Microsoft.Extensions.DependencyInjection;

namespace Odyssey.Client.Authentication.Services
{
    public interface ISignalRScopeFactory
    {
        IServiceScope CreateDisplayScope(Guid displayId);
        IServiceScope CreateDeviceScope(Guid deviceId);
        IServiceScope CreateScope(string userId);
    }
}
