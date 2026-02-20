using Odyssey.Domain.Core.Models;

namespace Odyssey.Domain.Core.Services
{
    public interface IGameRegistry<out TGameHandle> where TGameHandle : GameHandle
    {
        TGameHandle GetGame(string gameId);
        Task<Dictionary<string, string>> GetDisplayNamesAsync(string userId);
    }
}
