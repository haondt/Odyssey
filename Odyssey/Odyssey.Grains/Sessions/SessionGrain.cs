using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Core;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions;
using Odyssey.GrainInterfaces.Sessions.Models;
using Odyssey.GrainInterfaces.Sessions.Services;

namespace Odyssey.Grains.Sessions
{
    public abstract class SessionGrain<TBoard, TGameState> : Grain, ISessionGrain<TBoard, TGameState>
        where TBoard : IDataStorageData<TBoard>
        where TGameState : IDataStorageData<TGameState>
    {
        private readonly IDataStorageGrain<SessionState<TBoard>> _sessionStateGrain;
        private readonly IDataStorageGrain<SessionState<TBoard>> _gameStateGrain;

        protected SessionGrain(
            IDataStorageGrainFactory<SessionState<TBoard>> sessionStateGrainFactory,
            IDataStorageGrainFactory<TGameState> gameStateGrainFactory,
            ISessionGrainFactory<TBoard, TGameState> grainFactory)
        {
            var id = grainFactory.GetIdentity(this);
            _sessionStateGrain = sessionStateGrainFactory.GetGrain(id);
            _gameStateGrain = sessionStateGrainFactory.GetGrain(id);
        }


        public async Task SetState(int version, Optional<TBoard> board = default, Optional<List<SessionPlayer>> players = default)
        {
            if (!board.HasValue && !players.HasValue)
                return;

            var (sessionState, _) = await _sessionStateGrain.GetDataAsync();
            if (board.TryGetValue(out var b))
                sessionState.Board = b;
            if (players.TryGetValue(out var p))
                sessionState.Players = p;

            await _sessionStateGrain.SetDataAsync(sessionState, version);
        }

        public async Task ClearStateAsync()
        {
            await _sessionStateGrain.ClearDataAsync();
            await _gameStateGrain.ClearDataAsync();
        }

        public Task ClearGameStateAsync() => _gameStateGrain.ClearDataAsync();
    }
}
