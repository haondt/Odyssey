using Haondt.Web.Core.Attributes;
using Odyssey.UI.Core.Models;

namespace Odyssey.UI.Core.Attributes
{
    public class OdysseyRenderPageAttribute : RenderPageAttribute
    {
        public string Role { get; set; } = OdysseyRoles.None;
    }
}
