using Haondt.Core.Models;
using Microsoft.Extensions.Options;
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
        private readonly IPersistentState<TGameState> _gameState;
        private readonly SessionSettings _settings;
        private bool _gameStateDirty;
        private IGrainTimer _flushTimer = default!;

        protected SessionGrain(
            IDataStorageGrainFactory<SessionState<TBoard>> sessionStateGrainFactory,
            IPersistentState<TGameState> gameState,
            ISessionGrainFactory<TBoard, TGameState> grainFactory,
            IOptions<SessionSettings> options)
        {
            var id = grainFactory.GetIdentity(this);
            _sessionStateGrain = sessionStateGrainFactory.GetGrain(id);
            _gameState = gameState;
            _settings = options.Value;
        }

        public override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            _flushTimer = this.RegisterGrainTimer(FlushDirtyStateAsync, TimeSpan.FromMilliseconds(_settings.FlushIntervalMilliseconds), TimeSpan.FromMilliseconds(_settings.FlushIntervalMilliseconds));
            return base.OnActivateAsync(cancellationToken);
        }

        public override async Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
        {
            await FlushDirtyStateAsync();
            await base.OnDeactivateAsync(reason, cancellationToken);
        }

        private async Task FlushDirtyStateAsync()
        {
            if (_gameStateDirty)
            {
                _gameStateDirty = false;
                try
                {
                    await _gameState.WriteStateAsync();
                }
                catch
                {
                    _gameStateDirty = true;
                    throw;
                }
            }
        }

        public async Task<ReadOnlySessionState> GetStateAsync()
        {
            var (sessionState, version) = await _sessionStateGrain.GetDataAsync();
            return new ReadOnlySessionState
            {
                Version = version,
                Players = sessionState.Players.Select(q => new ReadOnlySessionPlayer
                {
                    Name = q.Name.ToString(),
                    Devices = q.Devices.ToHashSet()
                }).ToList()
            };
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
            await ClearGameStateAsync();
        }

        public async Task ClearGameStateAsync()
        {
            var oldGameStateDirty = _gameStateDirty;
            _gameStateDirty = false;
            try
            {
                await _gameState.ClearStateAsync();
            }
            catch
            {
                _gameStateDirty = oldGameStateDirty;
                throw;
            }
        }

        public Task<TGameState> GetGameStateAsync() => Task.FromResult(_gameState.State);

        public async Task WriteGameStateAsync(TGameState state)
        {
            _gameState.State = state;
            _gameStateDirty = true;
            await FlushDirtyStateAsync();
        }

        public Task SetGameStateAsync(TGameState state)
        {
            _gameState.State = state;
            _gameStateDirty = true;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Force game state to be persisted immediately.
        /// </summary>
        /// <returns></returns>
        public Task FlushAsync() => FlushDirtyStateAsync();

        public void Dispose() => _flushTimer?.Dispose();
    }
}
