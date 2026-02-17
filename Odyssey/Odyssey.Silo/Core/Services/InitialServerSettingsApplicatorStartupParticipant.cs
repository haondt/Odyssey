using Microsoft.Extensions.Options;
using Odyssey.Domain.Core.Models;
using Odyssey.Domain.Core.Services;
using Odyssey.Silo.Core.Models;

namespace Odyssey.Silo.Core.Services
{
    public class InitialServerSettingsApplicatorStartupParticipant(
        IOptions<InitialServerSettings> options,
        ICachedDataService<ServerSettings> settingsService) : ISiloStartupParticipant
    {
        public int Priority => 10000;

        public async Task OnStartupAsync()
        {
            var (existing, version) = await settingsService.GetDataAsync(ServerSettings.Key);
            var dirty = false;

            if (!existing.OpenRegistration.HasValue)
            {
                existing.OpenRegistration = options.Value.OpenRegistration;
                dirty = true;
            }

            if (dirty)
                await settingsService.SetDataAsync(ServerSettings.Key, existing, version);
        }
    }
}
