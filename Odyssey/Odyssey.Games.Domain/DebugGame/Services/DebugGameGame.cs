using Odyssey.Domain.Core.Models;
using Odyssey.Domain.Core.Services;
using Odyssey.Games.Domain.DebugGame.Models;

namespace Odyssey.Games.Domain.DebugGame.Services
{
    public class DebugGameGame(ICachedDataService<DebugGameGameSettings> gameSettings) : IGame
    {
        public string Identity => DebugGameConstants.Identity;

        public async ValueTask<GameSettings> GetSettingsAsync(string hostUserId) => (await gameSettings.GetDataAsync(hostUserId)).Data;
    }
}
