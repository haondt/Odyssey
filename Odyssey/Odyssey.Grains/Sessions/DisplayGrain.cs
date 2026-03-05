using Haondt.Core.Models;
using Odyssey.Domain.Core.Events;
using Odyssey.Domain.Sessions.Events;
using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions;
using Odyssey.GrainInterfaces.Sessions.Exceptions;
using Odyssey.GrainInterfaces.Sessions.Models;
using Odyssey.GrainInterfaces.Sessions.Reasons;
using Odyssey.Grains.Sessions.Models;
using Orleans.Streams;

namespace Odyssey.Grains.Sessions
{
    public class DisplayGrain : Grain, IDisplayGrain
    {
        private readonly IPersistentState<DisplayGrainState> _state;
        private readonly IGrainFactory<string, IJoinCodeGrain> _joinCodeGrainFactory;
        private readonly IAsyncStream<SignalROutboundEvent> _displayEventStream;

        public DisplayGrain(
            [PersistentState(nameof(DisplayGrainState), GrainConstants.GrainStorage)] IPersistentState<DisplayGrainState> state,
            IGrainFactory<string, IJoinCodeGrain> joinCodeGrainFactory,
            IGrainFactory<Guid, IDisplayGrain> grainFactory
            )
        {
            var id = grainFactory.GetIdentity(this);
            _state = state;
            _joinCodeGrainFactory = joinCodeGrainFactory;
            _displayEventStream = this.GetStreamProvider(GrainConstants.SignalRStreams)
                .GetStream<SignalROutboundEvent>(GrainConstants.HostEventsStreamNamespace, id);
        }

        public Task<PartyMemberProfile> GetMemberProfileAsync() => Task.FromResult<PartyMemberProfile>(_state.State.Profile);

        public Task<DisplayProfile> GetProfileAsync() => Task.FromResult(_state.State.Profile);

        public async Task<DetailedResult<IMemberPartyGrain, JoinPartyReason>> JoinPartyAsync(string joinCode)
        {
            if (_state.State.Party.TryGetValue(out var currentParty))
                return new(currentParty);

            try
            {
                // TODO: once you can deletestateonclear with ado.net, test this by trying to join a party that has been disbanded. it should work correctly instead of throwing nullref execeptions
                var joinCodeGrain = _joinCodeGrainFactory.GetGrain(joinCode);
                if (await joinCodeGrain.GetMemberPartyAsync() is not { HasValue: true, Value: var party })
                    return new(JoinPartyReason.PartyDoesNotExist);

                if (await party.JoinAsync(this.AsReference<IDisplayGrain>(), joinCode) != true)
                    return new(JoinPartyReason.PartyDoesNotExist);

                _state.State.Party = new(party);
                await _state.WriteStateAsync();
            }
            catch
            {
                _state.State.Party = new();
                throw;
            }
            return new(_state.State.Party.Value!);
        }

        public async Task LeavePartyAsync(string joinCode)
        {
            if (!_state.State.Party.TryGetValue(out var currentParty))
                return;

            await currentParty.LeaveAsync(this.AsReference<IDisplayGrain>());
            _state.State.Party = new();
            await _state.WriteStateAsync();
        }

        public async Task NotifyPartyDisbandedAsync(string joinCode)
        {
            _state.State.Party = new();
            try
            {
                await _state.WriteStateAsync();
            }
            finally
            {
                await _displayEventStream.OnNextAsync(new PartyDisbandedOutboundEvent { PartyId = joinCode });
            }
        }

        public async Task SetProfileAsync(DisplayProfile profile)
        {
            var oldProfile = _state.State.Profile;
            _state.State.Profile = profile;
            try
            {
                await _state.WriteStateAsync();
            }
            catch
            {
                _state.State.Profile = oldProfile;
                throw;
            }
        }

        public async Task<Optional<MemberPartyDetails>> GetMemberPartyAsync()
        {
            if (!_state.State.Party.TryGetValue(out var party))
                return new();
            try
            {
                return await party.GetPartyDetailsAsync(this.AsReference<IDisplayGrain>(), _state.State.Profile);
            }
            catch (NotPartyMemberException ex)
            {
                _state.State.Party = new();
                try
                {
                    await _state.WriteStateAsync();
                }
                catch (Exception ex2)
                {
                    throw new AggregateException(ex, ex2);
                }
                return new();
            }
        }

        public Task NotifyPartyMemberJoinedAsync()
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task NotifyPartyMemberLeftAsync()
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
