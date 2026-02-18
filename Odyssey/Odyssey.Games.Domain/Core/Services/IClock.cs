using Haondt.Core.Models;

namespace Odyssey.Games.Domain.Core.Services
{
    public interface IClock
    {
        public AbsoluteDateTime Now { get; }
    }
}
