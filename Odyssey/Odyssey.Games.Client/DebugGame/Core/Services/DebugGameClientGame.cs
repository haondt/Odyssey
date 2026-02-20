using Microsoft.AspNetCore.Components;
using Odyssey.Client.Core.Services;
using Odyssey.Domain.Core.Services;
using Odyssey.Games.Client.DebugGame.UI.Components;
using Odyssey.Games.Domain.DebugGame.Models;
using Odyssey.Games.Domain.DebugGame.Services;

namespace Odyssey.Games.Client.DebugGame.Core.Services
{
    public class DebugGameClientGame(
        ICachedDataRepository<DebugGameGameSettings> gameSettings,
        ICachedDataRepository<DebugGameBoard> boards,
        IBoardMetadataRepository boardMetadataRepository
        ) : DebugGameGame(gameSettings, boards, boardMetadataRepository), IClientGame
    {
        public async Task<IComponent> GetEditBoardComponentAsync(Guid boardId)
        {
            var (board, version) = await boards.GetDataAsync(boardId.ToString());
            return new DebugGameEditBoard
            {
                Board = board,
                Version = version
            };
        }
    }
}
