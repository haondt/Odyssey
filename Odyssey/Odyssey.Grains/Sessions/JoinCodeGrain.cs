using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions;

namespace Odyssey.Grains.Sessions
{
    public class JoinCodeGrain : Grain, IJoinCodeGrain
    {
        private readonly ICastedGrainFactory<string, IMemberPartyGrain> _partyGrainFactory;
        private readonly IPersistentState<Optional<string>> _state;

        public JoinCodeGrain(
            ICastedGrainFactory<string, IMemberPartyGrain> partyGrainFactory,
            [PersistentState(nameof(JoinCodeGrain), GrainConstants.GrainStorage)] IPersistentState<Optional<string>> state)
        {
            _partyGrainFactory = partyGrainFactory;
            _state = state;
        }


        public Task<bool> CheckOwnershipAsync(string ownerId) => Task.FromResult(_state.State.TryGetValue(out var current) && current == ownerId);

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

        public Task<Optional<IMemberPartyGrain>> GetMemberPartyAsync()
        {
            if (!_state.State.HasValue)
                return Task.FromResult(new Optional<IMemberPartyGrain>());

            var partyGrain = _partyGrainFactory.GetGrain(_state.State.Value);
            return Task.FromResult(new Optional<IMemberPartyGrain>(partyGrain));
        }

        public Task<Optional<string>> GetOwnerIdAsync() => Task.FromResult(_state.State);

        public async Task<Result> Release(string ownerId)
        {
            if (!_state.State.HasValue)
                return Result.Success;
            if (_state.State.Value != ownerId)
                return Result.Failure;

            await _state.ClearStateAsync();
            return Result.Success;
        }

        public Task DeactivateOnIdleAsync()
        {
            DeactivateOnIdle();
            return Task.CompletedTask;
        }

    }
}
