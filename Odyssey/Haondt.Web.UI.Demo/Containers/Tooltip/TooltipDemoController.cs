using Haondt.Web.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Haondt.Web.UI.Demo.Containers.Tooltip
{
    [Route("/demo/tooltip/")]
    public class TooltipDemoController : UIController
    {
        [HttpGet]
        public Task<IResult> Get() => ComponentFactory.RenderComponentAsync<TooltipDemo>();

    }
}
