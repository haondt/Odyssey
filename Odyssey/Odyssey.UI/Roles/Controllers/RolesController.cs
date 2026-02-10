using Haondt.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.UI.Core.Controllers;

namespace Odyssey.UI.Roles.Controllers
{
    [Route("/roles")]
    public class RolesController(IComponentFactory componentFactory) : UIController(componentFactory)
    {
        [HttpGet]
        [AllowAnonymous]
        public Task<IResult> Get() => _componentFactory.RenderComponentAsync<Components.Roles>();
    }
}
