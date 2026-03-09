using Microsoft.Extensions.DependencyInjection;
using Odyssey.Client.Authentication.Services;
using Odyssey.Client.Sessions.Models;

namespace Odyssey.Client.Host.Services
{
    public class ClientHostService(IHostSessionService sessionService, IServiceProvider serviceProvider) : IClientHostService
    {
        public async Task<ClientHostPartyHandle> GetPartyAsync()
        {
            var userId = await sessionService.GetUserIdAsync();
            return ActivatorUtilities.CreateInstance<ClientHostPartyHandle>(serviceProvider, userId);
        }
    }
}
