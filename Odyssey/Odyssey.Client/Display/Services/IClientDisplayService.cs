using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Sessions.Models;
using Odyssey.GrainInterfaces.Sessions.Reasons;

namespace Odyssey.Client.Display.Services
{
    public interface IClientDisplayService
    {
        Task<Optional<MemberPartyDetails>> GetPartyAsync();
        Task<DetailedResult<MemberPartyDetails, JoinPartyReason>> JoinPartyAsync(string joinCode);
        Task ConfigureDisplayProfile(DisplayProfile profile);
    }
}
