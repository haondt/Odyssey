using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Core;

namespace Odyssey.GrainInterfaces.Sessions
{
    public interface IJoinCodeGrain : IGrain<string>, IGrainWithStringKey
    {
        Task<Optional<string>> GetOwnerId();
        Task<Result> Claim(string ownerId);
        Task<bool> CheckOwnership(string ownerId);
        Task<Result> Release(string ownerId);
    }
}
