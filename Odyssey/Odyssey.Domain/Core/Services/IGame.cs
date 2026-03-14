using Odyssey.Core.Models;
using Odyssey.Domain.Core.Models;

namespace Odyssey.Domain.Core.Services
{
    public interface IGame : IGameBoardsService, IGameSettingsService, IGameSessionsService
    {
        string Id { get; }
    }

    public interface IGameSettingsService
    {
        Task<GameSettings> GetSettingsAsync(string hostUserId);
        Task<string> GetDisplayNameAsync(string userId);

    }

    public interface IGameBoardsService
    {
        Task<(Guid Id, BoardMetadata Metadata)> CreateBoardAsync(string ownerId, string name);
        Task DeleteBoardAsync(OwnedEntityGuid id);
    }

    public interface IGameSessionsService
    {
        Task<(Guid Id, SessionMetadata Metadata)> CreateSessionAsync(string ownerId, string name, Guid boardId, string boardName, bool ephemeral);
        Task DeleteSessionAsync(OwnedEntityGuid id);
        Task ResetSessionAsync(OwnedEntityGuid id);
    }
}
