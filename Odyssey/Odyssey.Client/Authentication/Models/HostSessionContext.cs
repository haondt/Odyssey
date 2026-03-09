using Haondt.Core.Models;

namespace Odyssey.Client.Authentication.Models
{
    public class HostSessionContext
    {
        public Optional<string> UserId { get; set; }
        public Optional<bool> IsAuthenticated { get; set; }
    }
}
