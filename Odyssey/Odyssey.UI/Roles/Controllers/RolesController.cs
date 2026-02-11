using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.UI.Core.Controllers;

namespace Odyssey.UI.Roles.Controllers
{
    [Route("/roles")]
    public class RolesController : UIController
    {
        [HttpGet]
        [AllowAnonymous]
        public Task<IResult> Get() => ComponentFactory.RenderComponentAsync<Components.Roles>();
    }
}
