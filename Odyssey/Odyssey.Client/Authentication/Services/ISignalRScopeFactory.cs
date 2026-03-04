using Microsoft.Extensions.DependencyInjection;

namespace Odyssey.Client.Authentication.Services
{
    public interface ISignalRScopeFactory
    {
        IServiceScope CreateScope(string userId);
    }
}
