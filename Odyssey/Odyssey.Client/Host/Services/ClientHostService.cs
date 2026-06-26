using Microsoft.Extensions.DependencyInjection;
using Odyssey.Client.Authentication.Services;
using Odyssey.Client.Core.Services;
using Odyssey.Domain.Core.Services;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions;
using Odyssey.GrainInterfaces.Sessions.Models;

namespace Odyssey.Client.Host.Services
{
    public class ClientHostService(
            IHostSessionService sessionService,
            ICastedGrainFactory<string, IHostPartyGrain> partyFactory,
            IGrainFactory<string, IHostGrain> grainFactory,
            IDeveloperDataSeeder developerDataSeeder) : IClientHostService
    {
        public async Task<IHostPartyGrain> GetPartyAsync()
        {
            var userId = await sessionService.GetUserIdAsync();
            return partyFactory.GetGrain(userId);
        }

        public async Task<HostSettings> GetHostSettingsAsync()
        {
            var userId = await sessionService.GetUserIdAsync();
            var grain = grainFactory.GetGrain(userId);
            return await grain.GetHostSettingsAsync();
        }

        public async Task SetHostSettingsAsync(HostSettings settings)
        {
            var userId = await sessionService.GetUserIdAsync();
            var grain = grainFactory.GetGrain(userId);
            await grain.SetHostSettingsAsync(settings);
        }

        public Task SeedDeveloperDataAsync() => developerDataSeeder.SeedAsync();
    }
}
