using Odyssey.Client.Display.Models;

namespace Odyssey.Client.Display.Services
{
    public class DisplaySessionService(DisplaySessionContext context) : IDisplaySessionService
    {
        public Guid DisplayId => context.DisplayId.Value;
        public bool IsAuthenticated => context.DisplayId.HasValue;
    }
}
