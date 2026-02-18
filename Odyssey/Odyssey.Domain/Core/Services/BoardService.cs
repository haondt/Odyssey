using Odyssey.Domain.Core.Models;

namespace Odyssey.Domain.Core.Services
{
    public class BoardService(IGameRegistry games) : IBoardService
    {
        public Task<(Guid Id, BoardMetadata Board)> CreateBoard(BoardCreationOptions options)
        {
            var game = games.GetGame(options.GameId);
            return game.CreateBoard(options.OwnerId, options.Name);
        }
    }
}
