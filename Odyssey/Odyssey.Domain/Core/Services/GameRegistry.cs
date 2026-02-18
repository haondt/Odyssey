using Odyssey.Domain.Core.Models;

namespace Odyssey.Domain.Core.Services
{
    public class GameRegistry(IEnumerable<IGame> games) : IGameRegistry
    {
        private readonly Dictionary<string, IGame> _games = games.ToDictionary(g => g.Id, g => g);

        public IGame GetGame(string gameId)
        {
            return _games[gameId];
        }

        public async Task<Dictionary<string, string>> GetGameNamesAsync(string userId)
        {
            var result = new Dictionary<string, string>();
            foreach (var game in games)
            {
                var gameSettings = await game.GetSettingsAsync(userId);
                result[game.Id] = gameSettings.DisplayName;
            }
            return result;
        }
    }
}
