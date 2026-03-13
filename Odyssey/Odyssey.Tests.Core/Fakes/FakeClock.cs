using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;

namespace Odyssey.Tests.Core.Fakes
{
    public class FakeClock : IClock
    {
        public Func<AbsoluteDateTime> NowFactory { get; set; } = static () => AbsoluteDateTime.Now;
        public AbsoluteDateTime Now => NowFactory();
    }
}
