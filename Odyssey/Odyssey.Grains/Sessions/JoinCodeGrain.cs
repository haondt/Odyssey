using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.GrainInterfaces.Sessions;

namespace Odyssey.Grains.Sessions
{
    public class JoinCodeGrain : IJoinCodeGrain
    {
        private readonly IPersistentState<Optional<string>> _state;

        public JoinCodeGrain(
            [PersistentState(nameof(JoinCodeGrain), GrainConstants.GrainStorage)] IPersistentState<Optional<string>> state)
        {
            _state = state;
        }

        public Task<bool> CheckOwnership(string ownerId) => Task.FromResult(_state.State.TryGetValue(out var current) && current == ownerId);

        public async Task<Result> Claim(string ownerId)
        {
            if (_state.State.HasValue)
            {
                if (_state.State.Value == ownerId)
                    return Result.Success;
                return Result.Failure;
            }

            _state.State = new(ownerId);
            await _state.WriteStateAsync();
            return Result.Success;
        }

        public Task<Optional<string>> GetOwnerId() => Task.FromResult(_state.State);

        public async Task<Result> Release(string ownerId)
        {
            if (!_state.State.HasValue)
                return Result.Success;
            if (_state.State.Value != ownerId)
                return Result.Failure;

            await _state.ClearStateAsync();
            return Result.Success;
        }
    }
}
