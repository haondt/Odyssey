using Haondt.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.UI.Core.Controllers;

namespace Odyssey.UI.Host.Controllers
{
    [Route("/roles/host")]
    public class HostController(IComponentFactory componentFactory) : UIController(componentFactory)
    {
        [HttpGet]
        public Task<IResult> Get() => _componentFactory.RenderComponentAsync<Components.Host>();
    }
}
