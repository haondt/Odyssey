namespace Odyssey.Client.Games.Services
{
    public interface IGameRegistry
    {
        ValueTask<Dictionary<string, string>> GetGamesAsync();
    }
}
