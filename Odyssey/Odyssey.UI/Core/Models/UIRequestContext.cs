using Haondt.Core.Models;

namespace Odyssey.UI.Core.Models
{
    public class UIRequestContext
    {
        public string BottomSheetRelayUri { get; set; } = OdysseyRoles.None;
        public string BottomSheetTargetUri { get; set; } = OdysseyRoles.None;
        public Optional<Type> BottomSheetTargetUriComponentType { get; set; }
    }
}
