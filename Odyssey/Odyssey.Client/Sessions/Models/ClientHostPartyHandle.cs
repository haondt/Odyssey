using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions;

namespace Odyssey.Client.Sessions.Models
{
    public class ClientHostPartyHandle(string userId, IGrainFactory<string, IHostPartyGrain> partyFactory)
    {
        private IHostPartyGrain _party = partyFactory.GetGrain(userId);
        public Task<string> GetJoinCodeAsync() => _party.GetJoinCodeAsync();
    }
}
