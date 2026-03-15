using Haondt.Web.Core.Attributes;
using Odyssey.UI.Core.Models;

namespace Odyssey.UI.Core.Attributes
{
    public class OdysseyRenderPageAttribute : RenderPageAttribute
    {
        public string Role { get; set; } = OdysseyRoles.None;
        /// <summary>
        /// Some pages for some reason randomly have issues with idiomorph. e.g. hyperscript breaking. If Morph == false, then we will just use regular swaps.
        /// </summary>
        public bool Morph { get; set; } = true;
    }
}
