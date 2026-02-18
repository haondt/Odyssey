using Odyssey.Domain.Core.Models;

namespace Odyssey.Domain.Core.Services
{
    public interface IGameRegistry
    {
        IGame GetGame(string gameId);
        Task<Dictionary<string, string>> GetGameNamesAsync(string userId);
    }
}
