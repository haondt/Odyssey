using Haondt.Core.Models;

namespace Odyssey.Domain.Core.Services
{
    public class Clock : IClock
    {
        public AbsoluteDateTime Now => AbsoluteDateTime.Now;
    }
}
