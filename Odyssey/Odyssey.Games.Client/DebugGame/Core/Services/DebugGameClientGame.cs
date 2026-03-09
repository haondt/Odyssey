using Haondt.Core.Models;
using Haondt.Web.Core.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Odyssey.Client.Core.Services;
using Odyssey.Domain.Core.Services;
using Odyssey.Games.Client.DebugGame.UI.Components;
using Odyssey.Games.Domain.DebugGame.Models;
using Odyssey.Games.Domain.DebugGame.Services;
using Odyssey.Persistence.Models;

namespace Odyssey.Games.Client.DebugGame.Core.Services
{
    public class DebugGameClientGame(
        ICachedDataRepository<DebugGameGameSettings> gameSettings,
        ICachedDataRepository<DebugGameBoard> boards,
        IBoardMetadataRepository boardMetadataRepository,
        IStandaloneModelBinder modelBinder,
        ILogger<DebugGameGame> logger
        ) : DebugGameGame(gameSettings, boards, boardMetadataRepository, logger), IClientGame
    {
        public async Task<IComponent> GetEditBoardComponentAsync(OwnedEntityGuid id)
        {
            var (board, version) = await boards.GetDataAsync(id);
            return new DebugGameEditBoard
            {
                Board = board,
                Version = version
            };
        }

        public async Task<IComponent> GetResetEditBoardComponentAsync(OwnedEntityGuid id)
        {
            var (board, version) = await boards.GetDataAsync(id);
            return new DebugGameEditBoard
            {
                Board = board,
                Version = version,
                HxSwap = true
            };
        }

        public async Task<Optional<IComponent>> HandleBoardStateUpdateAsync(OwnedEntityGuid id, HttpContext context)
        {
            var version = context.Request.Form.GetValue<int>("version");
            var model = await modelBinder.BindAndValidateFormAsync<DebugGameBoard, DebugGameEditBoard>(context);
            version = await boards.SetDataAsync(id, model, version);
            try
            {
                await boardMetadataRepository.TouchBoardMetadataAsync(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Caught exception while touching board metadata after updating board state");
            }

            return new DebugGameEditBoard
            {
                Board = model,
                Version = version,
                HxSwap = true
            };
        }
    }
}
