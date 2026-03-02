using Odyssey.GrainInterfaces.Core;

namespace Odyssey.GrainInterfaces.Sessions
{
    public interface IHostGrain : IGrain<string>, IGrainWithStringKey
    {
    }
}