using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.UI.Core.Controllers;
using Odyssey.UI.Core.Models;

namespace Odyssey.UI.Host.Controllers
{
    [Route(OdysseyRoutes.Roles.Host)]
    public class HostController : UIController
    {
        [HttpGet]
        public Task<IResult> Get() => ComponentFactory.RenderComponentAsync<Components.Host>();
    }
}
