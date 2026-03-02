using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.UI.Core.Components;
using Odyssey.UI.Core.Controllers;
using Odyssey.UI.Core.Models;

namespace Odyssey.UI.Fragments.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route(OdysseyRoutes.Fragments.Index)]
    //[TypeFilter<FragmentsExceptionFilter>()] // might still use this. it disables error page generation to avoid circular references
    public class FragmentsController : UIController
    {
        [HttpGet(OdysseyRoutes.Fragments.Websocket.Index)]
        public Task<IResult> GetWebsocket(string targetRole) => ComponentFactory.RenderComponentAsync(new Websocket
        {
            Role = targetRole
        });
    }
}
