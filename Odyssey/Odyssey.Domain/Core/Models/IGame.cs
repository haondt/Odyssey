namespace Odyssey.Domain.Core.Models
{
    public interface IGame
    {
        public string Identity { get; }
        public ValueTask<GameSettings> GetSettingsAsync(string hostUserId);
    }
}
