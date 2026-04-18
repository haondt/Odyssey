using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Odyssey.Domain.Core.Models;
using Odyssey.Domain.Core.Services;
using Odyssey.Games.Domain.Core.Services;
using Odyssey.Games.Domain.DebugGame.Models;
using Odyssey.GrainInterfaces.Sessions.Services;

namespace Odyssey.Games.Domain.DebugGame.Services
{
    public abstract class DebugGameGame(
        ICachedDataRepository<DebugGameGameSettings> gameSettings,
        ICachedDataRepository<DebugGameBoard> boards,
        IBoardMetadataRepository boardMetadataRepository,
        ISessionMetadataRepository sessionMetadataRepository,
        ILogger<DebugGameGame> logger,
        ISessionGrainFactory<DebugGameBoard, DebugGameGameState> sessionGrainFactory) : BaseGame<DebugGameBoard, DebugGameGameState>(boards, sessionGrainFactory, boardMetadataRepository, sessionMetadataRepository, logger), IGame
    {
        protected ICachedDataRepository<DebugGameGameSettings> gameSettings = gameSettings;
        public override string Id => DebugGameConstants.GameId;

        public override async Task<GameSettings> GetSettingsAsync(string ownerId) => (await gameSettings.GetDataAsync(ownerId)).Data;
    }

}
