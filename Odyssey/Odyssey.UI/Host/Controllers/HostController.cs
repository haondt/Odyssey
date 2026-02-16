using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.UI.Core.Controllers;
using Odyssey.UI.Core.Models;

namespace Odyssey.UI.Host.Controllers
{
    [Route(OdysseyRoutes.Roles.Host.Index)]
    public class HostController : UIController
    {
        [HttpGet]
        public IResult Get() => TypedResults.Redirect(OdysseyRoutes.Roles.Host.Party.Index);

        [HttpGet(OdysseyRoutes.Roles.Host.Party.Index)]
        public Task<IResult> GetParty() => ComponentFactory.RenderComponentAsync<Components.HostParty>();

        [HttpGet(OdysseyRoutes.Roles.Host.Sessions.Index)]
        public Task<IResult> GetSessions() => ComponentFactory.RenderComponentAsync<Components.HostSessions>();
    }
}
