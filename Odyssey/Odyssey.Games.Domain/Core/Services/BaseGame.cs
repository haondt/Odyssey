using Microsoft.Extensions.Logging;
using Odyssey.Core.Models;
using Odyssey.Domain.Core.Models;
using Odyssey.Domain.Core.Services;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions.Services;

namespace Odyssey.Games.Domain.Core.Services
{
    public abstract class BaseGame<TBoard, TGameState>(
        ICachedDataRepository<TBoard> boards,
        ISessionGrainFactory<TBoard, TGameState> sessionGrainFactory,
        IBoardMetadataRepository boardMetadataRepository,
        ISessionMetadataRepository sessionMetadataRepository,
        ILogger<BaseGame<TBoard, TGameState>> logger) : IGame
        where TBoard : IDataStorageData<TBoard>
        where TGameState : IDataStorageData<TGameState>
    {
        protected ICachedDataRepository<TBoard> boards = boards;
        protected ISessionGrainFactory<TBoard, TGameState> sessionGrainFactory = sessionGrainFactory;
        protected IBoardMetadataRepository boardMetadataRepository = boardMetadataRepository;
        protected ISessionMetadataRepository sessionMetadataRepository = sessionMetadataRepository;
        protected ILogger<BaseGame<TBoard, TGameState>> logger = logger;

        public abstract string Id { get; }
        public abstract Task<GameSettings> GetSettingsAsync(string hostUserId);

        public async Task<string> GetDisplayNameAsync(string ownerId)
        {
            var settings = await GetSettingsAsync(ownerId);
            return settings.DisplayName;
        }

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

        public async Task ResetSessionAsync(OwnedEntityGuid id)
        {
            var session = sessionGrainFactory.GetGrain(id);
            await session.ClearGameStateAsync();
        }

        public async Task UpdateGameStateFromSerializedAsync(OwnedEntityGuid id, string serializedGameState)
        {
            var state = JsonUtils.DeserializeObject<TGameState>(serializedGameState);
            var session = sessionGrainFactory.GetGrain(id);
            await session.WriteGameStateAsync(state);
        }

        public async Task<string> GetSerializedGameStateAsync(OwnedEntityGuid id)
        {
            var session = sessionGrainFactory.GetGrain(id);
            var state = await session.GetGameStateAsync();
            return JsonUtils.SerializeObject(state);
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
    }
}
