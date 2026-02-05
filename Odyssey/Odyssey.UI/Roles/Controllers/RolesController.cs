using Haondt.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.UI.Core.Controllers;

namespace Odyssey.UI.Roles.Controllers
{
    [Route("/roles")]
    public class RolesController(IComponentFactory componentFactory) : UIController(componentFactory)
    {
        [HttpGet]
        public Task<IResult> Get() => _componentFactory.RenderComponentAsync<Components.Roles>();
    }
}
