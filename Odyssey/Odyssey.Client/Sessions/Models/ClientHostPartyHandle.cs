using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions;
using Odyssey.GrainInterfaces.Sessions.Models;

namespace Odyssey.Client.Sessions.Models
{
    public class ClientHostPartyHandle(string userId, ICastedGrainFactory<string, IHostPartyGrain> partyFactory)
    {
        private readonly IHostPartyGrain _party = partyFactory.GetGrain(userId);
        public Task<string> GetJoinCodeAsync() => _party.GetJoinCodeAsync();
        public Task<HostPartyDetails> GetPartyDetailsAsync() => _party.GetPartyDetailsAsync();
        public Task SetHostDataAsync(HostPartyData data) => _party.SetHostDataAsync(data);
        public Task<HostPartyData> GetHostDataAsync() => _party.GetHostDataAsync();
        public Task ResetPartyAsync() => _party.ResetPartyAsync();
    }
}
