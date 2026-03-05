using Odyssey.GrainInterfaces.Core;
using Odyssey.GrainInterfaces.Sessions.Models;

namespace Odyssey.GrainInterfaces.Sessions
{
    public interface IDisplayGrain : IGrain<Guid>, IGrainWithGuidKey, IPartyMemberGrain
    {
        public Task SetProfileAsync(DisplayProfile profile);
        Task<DisplayProfile> GetProfileAsync();
    }
}
