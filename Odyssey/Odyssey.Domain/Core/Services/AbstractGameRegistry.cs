using Odyssey.Domain.Core.Models;

namespace Odyssey.Domain.Core.Services
{
    public abstract class AbstractGameRegistry<TGame, TGameHandle>(IEnumerable<TGame> games) : IGameRegistry<TGameHandle> where TGameHandle : GameHandle where TGame : IGame
    {
        protected readonly Dictionary<string, TGame> _games = games.ToDictionary(g => g.Id, g => g);

        public abstract TGameHandle GetGame(string gameId);

        public async Task<Dictionary<string, string>> GetDisplayNamesAsync(string userId)
        {
            var result = new Dictionary<string, string>();
            foreach (var game in games)
                result[game.Id] = await game.GetDisplayNameAsync(userId);
            return result;
        }
    }
}
