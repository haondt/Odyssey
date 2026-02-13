using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.UI.Core.Controllers;
using Odyssey.UI.Core.Models;

namespace Odyssey.UI.Roles.Controllers
{
    [Route(OdysseyRoutes.Roles.Index)]
    public class RolesController : UIController
    {
        [HttpGet]
        [AllowAnonymous]
        public Task<IResult> Get() => ComponentFactory.RenderComponentAsync<Components.Roles>();
    }
}
