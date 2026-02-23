using Haondt.Core.Models;

namespace Odyssey.GrainInterfaces.Core.Services
{
    public interface IClock
    {
        public AbsoluteDateTime Now { get; }
    }
}
