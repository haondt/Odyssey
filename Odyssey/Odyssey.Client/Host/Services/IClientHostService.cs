using Odyssey.GrainInterfaces.Sessions;

namespace Odyssey.Client.Host.Services
{
    public interface IClientHostService
    {
        Task<IHostPartyGrain> GetPartyAsync();
    }
}
