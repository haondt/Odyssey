using Microsoft.Extensions.Logging;
using Odyssey.Core.Models;
using Odyssey.Domain.Core.Models;
using Odyssey.Domain.Core.Services;
using Odyssey.Games.Domain.DebugGame.Models;
using Odyssey.GrainInterfaces.Sessions.Services;

namespace Odyssey.Games.Domain.DebugGame.Services
{
    public class DebugGameGame(
        ICachedDataRepository<DebugGameGameSettings> gameSettings,
        ICachedDataRepository<DebugGameBoard> boards,
        IBoardMetadataRepository boardMetadataRepository,
        ISessionMetadataRepository sessionMetadataRepository,
        ILogger<DebugGameGame> logger,
        ISessionGrainFactory<DebugGameBoard, DebugGameGameState> sessionGrainFactory) : IGame
    {
        protected ICachedDataRepository<DebugGameBoard> boards = boards;
        protected IBoardMetadataRepository boardMetadataRepository = boardMetadataRepository;
        protected ICachedDataRepository<DebugGameGameSettings> gameSettings = gameSettings;
        protected ILogger<DebugGameGame> logger = logger;
        protected ISessionGrainFactory<DebugGameBoard, DebugGameGameState> sessionGrainFactory = sessionGrainFactory;
        public string Id => DebugGameConstants.GameId;

        public async Task<string> GetDisplayNameAsync(string ownerId)
        {
            var settings = await GetSettingsAsync(ownerId);
            return settings.DisplayName;
        }

        public async Task<GameSettings> GetSettingsAsync(string ownerId) => (await gameSettings.GetDataAsync(ownerId)).Data;
        public async Task<(Guid Id, BoardMetadata Metadata)> CreateBoardAsync(string ownerId, string name)
        {
            // since we are not changing any defaults, board state can be initialized lazily by the grain activator
            return await boardMetadataRepository.CreateBoardMetadataAsync(Id, ownerId, name);
        }

        public async Task DeleteBoardAsync(OwnedEntityGuid id)
        {
            await boardMetadataRepository.DeleteBoardMetadataAsync(id);
            try
            {
                await boards.ClearDataAsync(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to delete board {BoardId} data after deleting metadata.", id);
            }
        }

        public async Task<(Guid Id, SessionMetadata Metadata)> CreateSessionAsync(string ownerId, string name, Guid boardId, string boardName, bool ephemeral)
        {
            var (id, meta) = await sessionMetadataRepository.CreateSessionMetadataAsync(Id, ownerId, name, boardId, boardName, ephemeral);
            var board = await boards.GetDataAsync(new OwnedEntityGuid(ownerId, boardId));
            var session = sessionGrainFactory.GetGrain((ownerId, id));
            await session.SetState(0, board: board.Data);
            return (id, meta);
        }

        public async Task DeleteSessionAsync(OwnedEntityGuid id)
        {
            await sessionMetadataRepository.DeleteSessionMetadataAsync(id);
            try
            {
                var session = sessionGrainFactory.GetGrain(id);
                await session.ClearStateAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to delete session {SessionId} data after deleting metadata.", id);
            }
        }

        public async Task ResetSessionAsync(OwnedEntityGuid id)
        {
            var session = sessionGrainFactory.GetGrain(id);
            await session.ClearGameStateAsync();
        }

    }
}
