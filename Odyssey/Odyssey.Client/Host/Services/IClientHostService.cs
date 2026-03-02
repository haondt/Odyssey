using Odyssey.Client.Sessions.Models;

namespace Odyssey.Client.Host.Services
{
    public interface IClientHostService
    {
        Task<ClientHostPartyHandle> GetPartyAsync();
    }
}