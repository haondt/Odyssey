using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Sessions.Models;

namespace Odyssey.Grains.Sessions
{
    public partial class PartyGrain
    {
        public Task<string> GetJoinCodeAsync() => Task.FromResult(_state.State.JoinCode);
        public async Task ResetPartyAsync()
        {
            var oldMembers = _state.State.Members.ToList();
            var oldJoinCode = _state.State.JoinCode;

            await _state.TryAndWriteStateAsync(async () =>
            {
                _state.State.Members.Clear();
                _state.State.HostData.DisplayData.Clear();
                _state.State.HostData.DeviceData.Clear();
            });

            try
            {
                await ClaimRandomJoinCodeAsync();
            }
            finally
            {
                _ = _hostGrain.NotifyPartyDisbandedAsync(oldJoinCode);
                foreach (var (_, member) in oldMembers)
                    _ = member.NotifyPartyDisbandedAsync(oldJoinCode);
            }
        }

        public async Task<HostPartyDetails> GetPartyDetailsAsync()
        {
            var details = new HostPartyDetails
            {
                JoinCode = _state.State.JoinCode,
                Members = [],
                Data = _state.State.HostData
            };

            foreach (var (memberId, member) in _state.State.Members)
            {
                var profile = await member.GetMemberProfileAsync();
                details.Members.Add((memberId, profile));
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

        public Task<HostDisplayData> GetDisplayDataAsync(PartyMemberId memberId)
        {
            if (!_state.State.HostData.DisplayData.TryGetValue(memberId, out var data))
                throw new KeyNotFoundException($"Member {memberId} not found in display party members.");
            return Task.FromResult(data);
        }
        public Task<HostDeviceData> GetDeviceDataAsync(PartyMemberId memberId)
        {
            if (!_state.State.HostData.DeviceData.TryGetValue(memberId, out var data))
                throw new KeyNotFoundException($"Member {memberId} not found in device party members.");
            return Task.FromResult(data);
        }

        public async Task UpdateDisplayDataAsync(PartyMemberId memberId, HostDisplayData data, bool upsert = false)
        {
            if (!upsert && !_state.State.HostData.DisplayData.ContainsKey(memberId))
                throw new KeyNotFoundException($"Member {memberId} not found in display party members.");

            await _state.TryAndWriteStateAsync(() =>
            {
                _state.State.HostData.DisplayData[memberId] = data;
            });

            NotifyPartyMembersThatPartyMemberWasModified();
        }

        public async Task UpdateDeviceDataAsync(PartyMemberId memberId, HostDeviceData data, bool upsert = false)
        {
            if (!upsert && !_state.State.HostData.DeviceData.ContainsKey(memberId))
                throw new KeyNotFoundException($"Member {memberId} not found in device party members.");

            await _state.TryAndWriteStateAsync(() =>
            {
                _state.State.HostData.DeviceData[memberId] = data;
            });

            NotifyPartyMembersThatPartyMemberWasModified();
        }

        public async Task RemoveMemberAsync(PartyMemberId memberId)
        {
            var memberIdx = _state.State.Members.FindIndex(q => q.Id == memberId);
            if (memberIdx == -1)
                return;
            var (_, member) = _state.State.Members[memberIdx];

            await _state.TryAndWriteStateAsync(() =>
            {
                _state.State.Members.RemoveAt(memberIdx);
                switch (memberId.Type)
                {
                    case PartyMemberType.Device:
                        _state.State.HostData.DeviceData.Remove(memberId);
                        break;
                    case PartyMemberType.Display:
                        _state.State.HostData.DisplayData.Remove(memberId);
                        break;
                }
            });

            _ = _hostGrain.NotifyPartyMemberLeftAsync();
            foreach (var (_, partyMember) in _state.State.Members)
                _ = partyMember.NotifyPartyMemberLeftAsync();
            _ = member.NotifyRemovedFromPartyAsync(_state.State.JoinCode);
        }
    }
}
