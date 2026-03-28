using Haondt.Core.Models;
using Microsoft.Extensions.Logging;
using Odyssey.Domain.Core.Events;
using Odyssey.Domain.Display.Events;
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
        private readonly Guid _id;
        private readonly IPersistentState<DisplayGrainState> _state;
        private readonly IGrainFactory<string, IJoinCodeGrain> _joinCodeGrainFactory;
        private readonly ILogger<DisplayGrain> _logger;
        private readonly IAsyncStream<SignalROutboundEvent> _displayEventStream;
        private readonly PartyMemberId _partyMemberId;

        public DisplayGrain(
            [PersistentState(nameof(DisplayGrainState), GrainConstants.GrainStorage)] IPersistentState<DisplayGrainState> state,
            IGrainFactory<string, IJoinCodeGrain> joinCodeGrainFactory,
            IGrainFactory<Guid, IDisplayGrain> grainFactory,
            ILogger<DisplayGrain> logger
            )
        {
            _id = grainFactory.GetIdentity(this);
            _state = state;
            _joinCodeGrainFactory = joinCodeGrainFactory;
            _logger = logger;
            _displayEventStream = this.GetStreamProvider(GrainConstants.SignalRStreams)
                .GetStream<SignalROutboundEvent>(GrainConstants.DisplayEventsStreamNamespace, _id);
            _partyMemberId = new PartyMemberId(_id, PartyMemberType.Display);
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

                if (await party.JoinAsync(_partyMemberId, this.AsReference<IDisplayGrain>(), joinCode) != true)
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

        public async Task<DetailedResult<LeavePartyReason>> LeavePartyAsync(string joinCode)
        {
            if (!_state.State.Party.TryGetValue(out var currentParty))
                return new(LeavePartyReason.PartyDoesNotExist);

            if (await currentParty.LeaveAsync(_partyMemberId, joinCode) == false)
                return new(LeavePartyReason.PartyDoesNotExist);

            _state.State.Party = new();
            await _state.WriteStateAsync();


            // we are no longer part of the party so we don't receive any notification about our own removal
            await _displayEventStream.OnNextAsync(new DisplaySelfLeftPartyOutboundEvent());

            return new();
        }

        public async Task NotifyPartyDisbandedAsync(string joinCode)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                using (_logger.BeginScope(new { DisplayId = _id }))
                    _logger.LogDebug("Received party {JoinCode} disbanded event", joinCode);
            }
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
                return await party.GetPartyDetailsAsync(_partyMemberId, _state.State.Profile);
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
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                using (_logger.BeginScope(new { DisplayId = _id }))
                    _logger.LogDebug("Received party member joined event");
            }
            return _displayEventStream.OnNextAsync(new PartyMemberJoinedOutboundEvent());
        }

        public Task NotifyPartyMemberLeftAsync()
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                using (_logger.BeginScope(new { DisplayId = _id }))
                    _logger.LogDebug("Received party member left event");
            }
            return _displayEventStream.OnNextAsync(new PartyMemberLeftOutboundEvent());
        }

        public Task NotifyPartyMemberModifiedAsync()
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                using (_logger.BeginScope(new { DeviceId = _id }))
                    _logger.LogDebug("Received party member modified event");
            }
            return _displayEventStream.OnNextAsync(new PartyMemberModifiedOutboundEvent());
        }
    }
}
