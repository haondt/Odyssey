using Odyssey.Domain.Core.Models;

namespace Odyssey.Domain.Core.Services
{
    public interface IGame : IGameBoardsService, IGameSettingsService
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
    }
}
