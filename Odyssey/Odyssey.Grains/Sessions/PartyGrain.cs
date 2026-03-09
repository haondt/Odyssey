using Haondt.Core.Models;
using Microsoft.Extensions.Logging;
using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions;
using Odyssey.GrainInterfaces.Sessions.Exceptions;
using Odyssey.GrainInterfaces.Sessions.Models;
using Odyssey.Grains.Sessions.Models;

namespace Odyssey.Grains.Sessions
{
    public class PartyGrain : Grain, IPartyGrain
    {
        private readonly IPersistentState<PartyGrainState> _state;
        private readonly string _id;
        private readonly ICrockfordService _crockford;
        private readonly IGrainFactory<string, IJoinCodeGrain> _joinCodeGrainFactory;
        private readonly IHostGrain _hostGrain;
        private readonly ILogger<PartyGrain> _logger;
        private const int _joinCodeSize = 5;

        public PartyGrain(
            [PersistentState(nameof(PartyGrainState), GrainConstants.GrainStorage)] IPersistentState<PartyGrainState> state,
            ICrockfordService crockford,
            IGrainFactory<string, IPartyGrain> grainFactory,
            IGrainFactory<string, IJoinCodeGrain> joinCodeGrainFactory,
            IGrainFactory<string, IHostGrain> hostGrainFactory,
            ILogger<PartyGrain> logger)
        {
            _state = state;
            _id = grainFactory.GetIdentity(this);
            _crockford = crockford;
            _joinCodeGrainFactory = joinCodeGrainFactory;
            _hostGrain = hostGrainFactory.GetGrain(_id);
            _logger = logger;
        }

        public Task<string> GetJoinCodeAsync() => Task.FromResult(_state.State.JoinCode);

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
                _state.State.JoinCode = joinCode;
                await _state.WriteStateAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _state.State.JoinCode = oldJoinCode;
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

        public async Task ResetPartyAsync()
        {
            var oldMembers = _state.State.Members.ToList();
            var oldJoinCode = _state.State.JoinCode;
            _state.State.Members.Clear();
            try
            {
                await ClaimRandomJoinCodeAsync();
            }
            catch
            {
                _state.State.Members = oldMembers;
                throw;
            }

            _ = _hostGrain.NotifyPartyDisbandedAsync(oldJoinCode);

            foreach (var member in oldMembers)
                _ = member.NotifyPartyDisbandedAsync(oldJoinCode);
        }

        public async Task<bool> LeaveAsync(IPartyMemberGrain member, Optional<string> joinCode = default)
        {
            if (!_state.State.Members.Contains(member))
                return true;

            if (joinCode.HasValue && _state.State.JoinCode != joinCode.Value)
                return false;

            _state.State.Members.Remove(member);
            try
            {
                await _state.WriteStateAsync();
            }
            catch
            {
                _state.State.Members.Add(member);
                throw;
            }

            _ = _hostGrain.NotifyPartyMemberLeftAsync();
            foreach (var partyMember in _state.State.Members)
                _ = partyMember.NotifyPartyMemberLeftAsync();

            return true;
        }

        private Task TryReleaseJoinCode(string joinCode)
        {
            var joinCodeGrain = _joinCodeGrainFactory.GetGrain(joinCode);
            return joinCodeGrain.Release(_id);
        }

        public async Task<bool> JoinAsync(IPartyMemberGrain member, string joinCode)
        {
            if (_state.State.Members.Contains(member))
                return true;

            if (_state.State.JoinCode != joinCode)
            {
                await TryReleaseJoinCode(joinCode);
                return false;
            }

            _state.State.Members.Add(member);
            try
            {
                await _state.WriteStateAsync();
            }
            catch
            {
                _state.State.Members.Remove(member);
                throw;
            }

            _ = _hostGrain.NotifyPartyMemberJoinedAsync();
            foreach (var partyMember in _state.State.Members)
                _ = partyMember.NotifyPartyMemberJoinedAsync();

            return true;
        }

        public async Task<MemberPartyDetails> GetPartyDetailsAsync(IPartyMemberGrain requester, PartyMemberProfile requesterProfile)
        {
            if (!_state.State.Members.Contains(requester))
                throw new NotPartyMemberException();

            var details = new MemberPartyDetails
            {
                JoinCode = _state.State.JoinCode,
                Members = []
            };

            var requesterGrainId = requester.GetGrainId();
            foreach (var member in _state.State.Members)
            {
                // avoid deadlock
                PartyMemberProfile profile;
                if (member.GetGrainId() == requesterGrainId)
                    profile = requesterProfile;
                else
                    profile = await member.GetMemberProfileAsync();

                details.Members.Add(profile);
            }

            return details;
        }

        public Task DeactivateOnIdleAsync()
        {
            DeactivateOnIdle();
            return Task.CompletedTask;
        }

        public async Task<HostPartyDetails> GetPartyDetailsAsync()
        {
            var details = new HostPartyDetails
            {
                JoinCode = _state.State.JoinCode,
                Members = [],
                Data = _state.State.HostData
            };

            foreach (var member in _state.State.Members)
            {
                var profile = await member.GetMemberProfileAsync();
                details.Members.Add(profile);
            }

            return details;
        }

        public async Task SetHostDataAsync(HostPartyData data)
        {
            _state.State.HostData = data;
            await _state.WriteStateAsync();
        }

        public Task<HostPartyData> GetHostDataAsync()
        {
            return Task.FromResult(_state.State.HostData);
        }
    }
}
