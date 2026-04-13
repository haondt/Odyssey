using Microsoft.Extensions.DependencyInjection;
using Odyssey.Client.Authentication.Services;
using Odyssey.Domain.Core.Services;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions;

namespace Odyssey.Client.Host.Services
{
    public class ClientHostService(IHostSessionService sessionService, ICastedGrainFactory<string, IHostPartyGrain> partyFactory) : IClientHostService
    {
        public async Task<IHostPartyGrain> GetPartyAsync()
        {
            var userId = await sessionService.GetUserIdAsync();
            return partyFactory.GetGrain(userId);
        }
    }
}
