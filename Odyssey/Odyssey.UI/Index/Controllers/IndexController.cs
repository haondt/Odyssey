using Haondt.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.UI.Core.Controllers;

namespace Odyssey.UI.Index.Controllers
{
    [Route("/")]
    public class IndexController(IComponentFactory componentFactory) : UIController(componentFactory)
    {
        [HttpGet]
        [AllowAnonymous]
        public IResult Get() => TypedResults.Redirect("/roles");
    }
}
