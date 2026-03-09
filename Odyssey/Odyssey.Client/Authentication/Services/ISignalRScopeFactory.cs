using Microsoft.Extensions.DependencyInjection;

namespace Odyssey.Client.Authentication.Services
{
    public interface ISignalRScopeFactory
    {
        IServiceScope CreateDisplayScope(Guid displayId);
        IServiceScope CreateScope(string userId);
    }
}
