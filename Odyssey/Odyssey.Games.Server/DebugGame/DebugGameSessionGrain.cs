using Microsoft.Extensions.Options;
using Odyssey.Games.Domain.DebugGame.Models;
using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions.Models;
using Odyssey.GrainInterfaces.Sessions.Services;
using Odyssey.Grains.Sessions;

namespace Odyssey.Games.Server.DebugGame
{
    public class DebugGameSessionGrain : SessionGrain<DebugGameBoard, DebugGameGameState>
    {
        public DebugGameSessionGrain(
            IDataStorageGrainFactory<SessionState<DebugGameBoard>> sessionStateGrainFactory,
            [PersistentState(nameof(DebugGameGameState), GrainConstants.GrainStorage)]
            IPersistentState<DebugGameGameState> gameState,
            ISessionGrainFactory<DebugGameBoard, DebugGameGameState> grainFactory,
            IOptions<SessionSettings> options) : base(sessionStateGrainFactory, gameState, grainFactory, options)
        {
        }


    }
}
