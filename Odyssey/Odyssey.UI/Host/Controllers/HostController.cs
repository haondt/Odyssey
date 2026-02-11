using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.UI.Core.Controllers;

namespace Odyssey.UI.Host.Controllers
{
    [Route("/roles/host")]
    public class HostController : UIController
    {
        [HttpGet]
        public Task<IResult> Get() => ComponentFactory.RenderComponentAsync<Components.Host>();
    }
}
