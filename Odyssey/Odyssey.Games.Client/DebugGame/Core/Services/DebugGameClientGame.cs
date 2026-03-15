using Haondt.Core.Models;
using Haondt.Web.Core.Extensions;
using Haondt.Web.UI.Components.Element;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Odyssey.Client.Core.Services;
using Odyssey.Core.Models;
using Odyssey.Domain.Core.Services;
using Odyssey.Games.Client.DebugGame.UI.Components;
using Odyssey.Games.Domain.DebugGame.Models;
using Odyssey.Games.Domain.DebugGame.Services;
using Odyssey.GrainInterfaces.Sessions.Services;

namespace Odyssey.Games.Client.DebugGame.Core.Services
{
    public class DebugGameClientGame(
        ICachedDataRepository<DebugGameGameSettings> gameSettings,
        ICachedDataRepository<DebugGameBoard> boards,
        IBoardMetadataRepository boardMetadataRepository,
        ISessionMetadataRepository sessionMetadataRepository,
        IStandaloneModelBinder modelBinder,
        ILogger<DebugGameGame> logger,
        ISessionGrainFactory<DebugGameBoard, DebugGameGameState> sessionGrainFactory
        ) : DebugGameGame(gameSettings, boards, boardMetadataRepository, sessionMetadataRepository, logger, sessionGrainFactory), IClientGame
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
            var model = await modelBinder.BindAndValidateFormAsync<DebugGameBoard, FieldInvalidator>(context);
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
        public async Task<(IComponent PlayersSummary, IComponent GameSummary)> GetSessionSummaryComponentAsync(OwnedEntityGuid id)
        {
            var session = sessionGrainFactory.GetGrain(id);
            var gameState = await session.GetGameStateAsync();
            var sessionState = await session.GetStateAsync();
            return
            (
                new DebugGamePlayersSummary
                {
                    GameState = gameState,
                    SessionState = sessionState
                },
                new DebugGameGameSummary
                {
                    GameState = gameState
                }
            );
        }

        public async Task<IComponent> GetEditGameStateComponentAsync(OwnedEntityGuid id)
        {
            var session = sessionGrainFactory.GetGrain(id);
            var gameState = await session.GetGameStateAsync();
            return new DebugGameEditGameState
            {
                GameState = gameState
            };
        }

        public async Task<Optional<IComponent>> HandleGameStateUpdateAsync(OwnedEntityGuid id, HttpContext context)
        {
            //var model = await modelBinder.BindAndValidateFormAsync<DebugGameGameState, DebugGameEditGameState>(context);
            var model = await modelBinder.BindAndValidateFormAsync<DebugGameGameState, FieldInvalidator>(context);
            var session = sessionGrainFactory.GetGrain(id);
            await session.WriteGameStateAsync(model);

            return new();
        }

        public Task<IComponent> GetResetEditGameStateComponentAsync(OwnedEntityGuid id)
        {
            throw new NotImplementedException();
        }
    }
}
