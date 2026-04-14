using Haondt.Core.Extensions;
using Haondt.Core.Models;
using Haondt.Orleans.Persistence;
using Microsoft.Extensions.Logging;
using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions;
using Odyssey.GrainInterfaces.Sessions.Exceptions;
using Odyssey.GrainInterfaces.Sessions.Models;
using Odyssey.Grains.Sessions.Models;

namespace Odyssey.Grains.Sessions
{
    public partial class PartyGrain : Grain, IPartyGrain
    {
        private readonly IRewindablePersistentState<PartyGrainState> _state;
        private readonly string _id;
        private readonly ICrockfordService _crockford;
        private readonly IGrainFactory<string, IJoinCodeGrain> _joinCodeGrainFactory;
        private readonly IHostGrain _hostGrain;
        private readonly ILogger<PartyGrain> _logger;
        private const int _joinCodeSize = 5;

        public PartyGrain(
            IRewindablePersistentStateFactory persistentStateFactory,
            ICrockfordService crockford,
            IGrainFactory<string, IPartyGrain> grainFactory,
            IGrainFactory<string, IJoinCodeGrain> joinCodeGrainFactory,
            IGrainFactory<string, IHostGrain> hostGrainFactory,
            ILogger<PartyGrain> logger)
        {
            _state = persistentStateFactory.Create<PartyGrainState>(GrainContext, nameof(PartyGrainState), GrainConstants.GrainStorage);
            _id = grainFactory.GetIdentity(this);
            _crockford = crockford;
            _joinCodeGrainFactory = joinCodeGrainFactory;
            _hostGrain = hostGrainFactory.GetGrain(_id);
            _logger = logger;
        }


        private async Task ClaimRandomJoinCodeAsync(CancellationToken cancellationToken = default)
        {
            var joinCode = _crockford.Random(_joinCodeSize);
            if (await ClaimJoinCode(joinCode, cancellationToken) is not { IsSuccessful: true })
                throw new InvalidOperationException("Failed to secure a random join code");
        }

        private async Task<Result> ClaimJoinCode(string joinCode, CancellationToken cancellationToken = default)
        {
            var joinCodeGrain = _joinCodeGrainFactory.GetGrain(joinCode);
            var currentOwner = await joinCodeGrain.GetOwnerIdAsync();

            if (await joinCodeGrain.Claim(_id) is not { IsSuccessful: true })
                return Result.Failure;

            var oldJoinCode = _state.State.JoinCode;
            if (oldJoinCode == joinCode)
                return Result.Success;

            try
            {
                await _state.TryAndWriteStateAsync(() =>
                {
                    _state.State.JoinCode = joinCode;
                });
            }
            catch (Exception ex)
            {
                if (!currentOwner.HasValue)
                {
                    try
                    {
                        await joinCodeGrain.Release(_id);
                    }
                    catch (Exception ex2)
                    {
                        throw new AggregateException(ex, ex2);
                    }
                }
                throw;
            }

            if (oldJoinCode is not null)
            {
                try
                {
                    var oldJoinCodeGrain = _joinCodeGrainFactory.GetGrain(oldJoinCode);
                    await oldJoinCodeGrain.Release(_id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to release old join code grain {OldJoinCode} from party {Party}", oldJoinCode, _id);
                }
            }

            return Result.Success;
        }

        public override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            if (!_state.RecordExists)
            {
                await ClaimRandomJoinCodeAsync(cancellationToken);
            }
            else
            {
                if (await ClaimJoinCode(_state.State.JoinCode, cancellationToken) is not { IsSuccessful: true })
                    await ResetPartyAsync();
            }
            await base.OnActivateAsync(cancellationToken);
        }


        public async Task<bool> LeaveAsync(PartyMemberId memberId, Optional<string> joinCode = default)
        {
            var memberIdx = _state.State.Members.FindIndex(q => q.Id == memberId);
            if (memberIdx == -1)
                return true;

            if (joinCode.HasValue && _state.State.JoinCode != joinCode.Value)
                return false;

            await _state.TryAndWriteStateAsync(() =>
            {
                _state.State.Members.RemoveAt(memberIdx);
                switch (memberId.Type)
                {
                    case PartyMemberType.Device:
                        _state.State.HostData.DeviceData.Remove(memberId);
                        foreach (var (k, v) in _state.State.HostData.DeviceData)
                            if (v.PlayerAssignmentDelegatedTo.Map(q => q == memberId).Or(false))
                                v.PlayerAssignmentDelegatedTo = new();
                        break;
                    case PartyMemberType.Display:
                        _state.State.HostData.DisplayData.Remove(memberId);
                        break;
                }
            });

            _ = _hostGrain.NotifyPartyMemberLeftAsync();
            foreach (var (_, partyMember) in _state.State.Members)
                _ = partyMember.NotifyPartyMemberLeftAsync();

            return true;
        }

        private void NotifyPartyMembersThatPartyMemberWasModified()
        {
            _ = _hostGrain.NotifyPartyMemberModifiedAsync();
            foreach (var (_, partyMember) in _state.State.Members)
                _ = partyMember.NotifyPartyMemberModifiedAsync();
        }

        private Task<Result> TryReleaseJoinCode(string joinCode)
        {
            var joinCodeGrain = _joinCodeGrainFactory.GetGrain(joinCode);
            return joinCodeGrain.Release(_id);
        }

        public async Task<bool> JoinAsync(PartyMemberId memberId, IPartyMemberGrain member, string joinCode)
        {
            if (_state.State.Members.Any(q => q.Id == memberId))
                return true;

            if (_state.State.JoinCode != joinCode)
            {
                await TryReleaseJoinCode(joinCode);
                return false;
            }

            await _state.TryAndWriteStateAsync(() =>
            {
                _state.State.Members.Add((memberId, member));
                switch (memberId.Type)
                {
                    case PartyMemberType.Device:
                        _state.State.HostData.DeviceData[memberId] = new();
                        break;
                    case PartyMemberType.Display:
                        _state.State.HostData.DisplayData[memberId] = new();
                        break;
                }
            });

            _ = _hostGrain.NotifyPartyMemberJoinedAsync();
            foreach (var (_, partyMember) in _state.State.Members)
                _ = partyMember.NotifyPartyMemberJoinedAsync();

            return true;
        }

        public async Task<MemberPartyDetails> GetPartyDetailsAsync(PartyMemberId requesterId, PartyMemberProfile requesterProfile)
        {
            if (!_state.State.Members.Any(q => q.Id == requesterId))
                throw new NotPartyMemberException();

            var details = new MemberPartyDetails
            {
                JoinCode = _state.State.JoinCode,
                Members = []
            };

            foreach (var (memberId, member) in _state.State.Members)
            {
                // avoid deadlock
                PartyMemberProfile profile;
                if (memberId == requesterId)
                    profile = requesterProfile;
                else
                    profile = await member.GetMemberProfileAsync();

                details.Members.Add((memberId, profile));
            }

            return details;
        }

        public Task DeactivateOnIdleAsync()
        {
            DeactivateOnIdle();
            return Task.CompletedTask;
        }

        public async Task ClearCurrentSessionAsync(Optional<Guid> sessionId = default)
        {
            if (!_state.State.CurrentSession.HasValue)
                return;

            if (sessionId.HasValue && sessionId.Value != _state.State.CurrentSession.Value.SessionId)
                throw new InvalidOperationException($"Given session ID {sessionId} does not expect the known session ID {_state.State.CurrentSession.Value.SessionId}");

            await _state.TryAndWriteStateAsync(() =>
            {
                _state.State.CurrentSession = default;
            });
        }

        public Task<Optional<(string GameId, Guid SessionId, SessionStatus Status)>> GetCurrentSessionAsync()
        {
            return Task.FromResult(_state.State.CurrentSession);
        }

        public async Task SetCurrentSessionAsync(string gameId, Guid sessionId, SessionStatus status)
        {
            await _state.TryAndWriteStateAsync(() =>
            {
                _state.State.CurrentSession = (gameId, sessionId, status);
            });

            // _ = _hostGrain.NotifySessionStatusChangedAsync(gameId, sessionId, status);
            // foreach (var (_, partyMember) in _state.State.Members)
            //     _ = partyMember.NotifySessionStatusChangedAsync(gameId, sessionId, status);
        }
    }
}
