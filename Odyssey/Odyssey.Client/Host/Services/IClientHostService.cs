using Odyssey.GrainInterfaces.Sessions;
using Odyssey.GrainInterfaces.Sessions.Models;

namespace Odyssey.Client.Host.Services
{
    public interface IClientHostService
    {
        Task<IHostPartyGrain> GetPartyAsync();
        Task SetHostSettingsAsync(HostSettings settings);
        Task<HostSettings> GetHostSettingsAsync();
        Task SeedDeveloperDataAsync();
    }
}
