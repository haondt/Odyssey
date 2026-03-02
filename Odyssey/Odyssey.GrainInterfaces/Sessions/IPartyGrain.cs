using Odyssey.GrainInterfaces.Core;

namespace Odyssey.GrainInterfaces.Sessions
{
    public interface IPartyGrain : IHostPartyGrain, IMemberPartyGrain
    {
    }
    public interface IHostPartyGrain : IGrain<string>, IGrainWithStringKey
    {
        Task<string> GetJoinCodeAsync();
        Task ResetPartyAsync();
    }

    public interface IMemberPartyGrain : IGrain<string>, IGrainWithStringKey
    {
    }
}
