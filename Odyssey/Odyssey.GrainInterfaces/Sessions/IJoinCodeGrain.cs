using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Core;
using Odyssey.GrainInterfaces.Testing;
using Orleans.Concurrency;

namespace Odyssey.GrainInterfaces.Sessions
{
    public interface IJoinCodeGrain : IGrain<string>, IGrainWithStringKey, IDeactivatableGrain
    {
        [AlwaysInterleave]
        Task<Optional<string>> GetOwnerIdAsync();
        Task<Result> Claim(string ownerId);
        Task<bool> CheckOwnershipAsync(string ownerId);
        Task<Result> Release(string ownerId);
        Task<Optional<IMemberPartyGrain>> GetMemberPartyAsync();
    }
}
