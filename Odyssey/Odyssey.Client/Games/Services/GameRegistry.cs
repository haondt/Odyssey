using Haondt.Core.Extensions;
using Odyssey.Client.Authentication.Services;
using Odyssey.Domain.Core.Models;

namespace Odyssey.Client.Games.Services
{
    public class GameRegistry(IEnumerable<IGame> games, ISessionService sessionService) : IGameRegistry
    {
        public async ValueTask<Dictionary<string, string>> GetGamesAsync()
        {
            var result = new Dictionary<string, string>();
            foreach (var game in games)
            {
                var gameSettings = await game.GetSettingsAsync((await sessionService.GetUserIdAsync()).Expect());
                result[game.Identity] = gameSettings.DisplayName;
            }
            return result;
        }
    }
}
