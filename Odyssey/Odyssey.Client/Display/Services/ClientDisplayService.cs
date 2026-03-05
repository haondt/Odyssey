using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions;
using Odyssey.GrainInterfaces.Sessions.Models;
using Odyssey.GrainInterfaces.Sessions.Reasons;

namespace Odyssey.Client.Display.Services
{
    public class ClientDisplayService(IDisplaySessionService sessionService, IGrainFactory<Guid, IDisplayGrain> grainFactory) : IClientDisplayService
    {
        public Task ConfigureDisplayProfile(DisplayProfile profile)
        {
            var grain = grainFactory.GetGrain(sessionService.DisplayId);
            return grain.SetProfileAsync(profile);
        }

        public Task<Optional<MemberPartyDetails>> GetPartyAsync()
        {
            var grain = grainFactory.GetGrain(sessionService.DisplayId);
            return grain.GetMemberPartyAsync();
        }

        public async Task<DetailedResult<MemberPartyDetails, JoinPartyReason>> JoinPartyAsync(string joinCode)
        {
            var grain = grainFactory.GetGrain(sessionService.DisplayId);
            var joinResult = await grain.JoinPartyAsync(joinCode);
            if (!joinResult.IsSuccessful)
                return new(joinResult.Reason);

            var partyDetails = await joinResult.Value.GetPartyDetailsAsync(grain, await grain.GetMemberProfileAsync());
            return new(partyDetails);
        }
    }
}
