using Haondt.Core.Models;

namespace Odyssey.Domain.Core.Services
{
    public interface IClock
    {
        public AbsoluteDateTime Now { get; }
    }
}
