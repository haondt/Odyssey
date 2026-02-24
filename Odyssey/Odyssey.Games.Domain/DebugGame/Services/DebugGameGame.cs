using Microsoft.Extensions.Logging;
using Odyssey.Domain.Core.Models;
using Odyssey.Domain.Core.Services;
using Odyssey.Games.Domain.DebugGame.Models;
using Odyssey.Persistence.Models;

namespace Odyssey.Games.Domain.DebugGame.Services
{
    public class DebugGameGame(
        ICachedDataRepository<DebugGameGameSettings> gameSettings,
        ICachedDataRepository<DebugGameBoard> boards,
        IBoardMetadataRepository boardMetadataRepository,
        ILogger<DebugGameGame> logger) : IGame
    {
        protected ICachedDataRepository<DebugGameBoard> boards = boards;
        protected IBoardMetadataRepository boardMetadataRepository = boardMetadataRepository;
        protected ICachedDataRepository<DebugGameGameSettings> gameSettings = gameSettings;
        protected ILogger<DebugGameGame> logger = logger;
        public string Id => DebugGameConstants.GameId;

        public async Task<string> GetDisplayNameAsync(string ownerId)
        {
            var settings = await GetSettingsAsync(ownerId);
            return settings.DisplayName;
        }

        public async Task<GameSettings> GetSettingsAsync(string ownerId) => (await gameSettings.GetDataAsync(ownerId)).Data;
        public async Task<(Guid Id, BoardMetadata Metadata)> CreateBoardAsync(string ownerId, string name)
        {
            var (id, meta) = await boardMetadataRepository.CreateBoardMetadataAsync(Id, ownerId, name);
            await boards.SetDataAsync(new OwnedEntityId<Guid>(ownerId, id).StringValue, new(), 0);
            return (id, meta);
        }

    }
}
