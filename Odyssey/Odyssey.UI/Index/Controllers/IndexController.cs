using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.UI.Core.Controllers;
using Odyssey.UI.Core.Models;

namespace Odyssey.UI.Index.Controllers
{
    [Route(OdysseyRoutes.Index)]
    public class IndexController : UIController
    {
        [HttpGet]
        [AllowAnonymous]
        public IResult Get() => TypedResults.Redirect(OdysseyRoutes.Home);
    }
}
