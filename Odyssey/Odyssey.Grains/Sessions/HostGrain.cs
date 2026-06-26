using Haondt.Orleans.Persistence;
using Microsoft.Extensions.Logging;
using Odyssey.Domain.Core.Events;
using Odyssey.Domain.Sessions.Events;
using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions;
using Odyssey.GrainInterfaces.Sessions.Models;
using Odyssey.Grains.Sessions.Models;
using Orleans.Streams;

namespace Odyssey.Grains.Sessions
{
    public class HostGrain : Grain, IHostGrain
    {
        private readonly string _id;
        private readonly IHostPartyGrain _party;
        private readonly IAsyncStream<SignalROutboundEvent> _hostEventStream;
        private readonly ILogger<HostGrain> _logger;
        private readonly IRewindablePersistentState<HostGrainState> _state;

        public HostGrain(IGrainFactory<string, IHostGrain> grainFactory,
            IRewindablePersistentStateFactory persistentStateFactory,
            ICastedGrainFactory<string, IHostPartyGrain> partyGrainFactory,
            ILogger<HostGrain> logger)
        {
            _id = grainFactory.GetIdentity(this);
            _party = partyGrainFactory.GetGrain(_id);
            _hostEventStream = this.GetStreamProvider(GrainConstants.SignalRStreams)
                .GetStream<SignalROutboundEvent>(GrainConstants.HostEventsStreamNamespace, _id);
            _logger = logger;
            _state = persistentStateFactory.Create<HostGrainState>(GrainContext, nameof(HostGrainState), GrainConstants.GrainStorage);
        }

        public async Task SetHostSettingsAsync(HostSettings settings)
        {
            _state.State.Settings = settings;
            await _state.WriteStateAsync();
        }

        public Task<HostSettings> GetHostSettingsAsync()
        {
            return Task.FromResult(_state.State.Settings);
        }

        public async Task NotifyPartyDisbandedAsync(string joinCode)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                using (_logger.BeginScope(new { HostId = _id }))
                    _logger.LogDebug("Received party {JoinCode} disbanded event", joinCode);
            }
            await _hostEventStream.OnNextAsync(new PartyDisbandedOutboundEvent { PartyId = joinCode });
        }


        public async Task NotifyPartyMemberJoinedAsync()
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                using (_logger.BeginScope(new { DisplayId = _id }))
                    _logger.LogDebug("Received party member joined event");
            }

            await _hostEventStream.OnNextAsync(new PartyMemberJoinedOutboundEvent());
        }

        public Task NotifyPartyMemberLeftAsync()
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                using (_logger.BeginScope(new { DisplayId = _id }))
                    _logger.LogDebug("Received party member left event");
            }
            return _hostEventStream.OnNextAsync(new PartyMemberLeftOutboundEvent());
        }

        public Task NotifyPartyMemberModifiedAsync()
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                using (_logger.BeginScope(new { DeviceId = _id }))
                    _logger.LogDebug("Received party member modified event");
            }
            return _hostEventStream.OnNextAsync(new PartyMemberModifiedOutboundEvent());
        }
    }
}
