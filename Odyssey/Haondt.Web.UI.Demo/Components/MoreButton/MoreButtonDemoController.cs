using Haondt.Web.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Haondt.Web.UI.Demo.Components.MoreButton
{
    [Route("/demo/more-button/")]
    public class MoreButtonDemoController : UIController
    {
        [HttpGet]
        public Task<IResult> Get() => ComponentFactory.RenderComponentAsync<MoreButtonDemo>();
    }
}
