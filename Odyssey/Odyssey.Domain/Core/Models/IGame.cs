namespace Odyssey.Domain.Core.Models
{
    public interface IGame
    {
        public string Id { get; }
        public Task<GameSettings> GetSettingsAsync(string hostUserId);
        Task<(Guid Id, BoardMetadata Metadata)> CreateBoard(string ownerId, string name);
    }
}
