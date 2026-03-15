using Haondt.Web.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Haondt.Web.UI.Demo
{
    [Route("/")]
    public class IndexController : UIController
    {
        [HttpGet]
        public Task<IResult> Get() => ComponentFactory.RenderComponentAsync<Index>();
    }
}