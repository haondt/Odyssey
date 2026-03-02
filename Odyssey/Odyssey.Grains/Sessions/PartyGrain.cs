using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.GrainInterfaces.Sessions;
using Odyssey.Grains.Sessions.Models;

namespace Odyssey.Grains.Sessions
{
    public class PartyGrain : Grain, IPartyGrain
    {
        private readonly IPersistentState<PartyGrainState> _state;

        public PartyGrain(
            [PersistentState(nameof(PartyGrainState), GrainConstants.GrainStorage)] IPersistentState<PartyGrainState> state)
        {
            _state = state;
        }

        public Task<string> GetJoinCodeAsync() => Task.FromResult(_state.State.JoinCode);

        public override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            if (!_state.RecordExists)
            {
                _state.State = new()
                {
                    JoinCode = "TODO!"
                };
                await _state.WriteStateAsync(cancellationToken);
            }
            await base.OnActivateAsync(cancellationToken);
        }

        public async Task ResetPartyAsync()
        {
            var oldMembers = _state.State.Members.ToList();
            var oldJoinCode = _state.State.JoinCode;
            _state.State.Members.Clear();
            _state.State.JoinCode = "ALSO TODO!";
            await _state.WriteStateAsync();

            foreach (var member in oldMembers)
                _ = member.NotifyPartyDisbandedAsync(oldJoinCode);
        }

    }
}
