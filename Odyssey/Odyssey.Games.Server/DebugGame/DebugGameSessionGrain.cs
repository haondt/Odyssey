using Odyssey.Games.Domain.DebugGame.Models;
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
            IDataStorageGrainFactory<DebugGameGameState> gameStateGrainFactory,
            ISessionGrainFactory<DebugGameBoard, DebugGameGameState> grainFactory) : base(sessionStateGrainFactory, gameStateGrainFactory, grainFactory)
        {
        }


    }
}
