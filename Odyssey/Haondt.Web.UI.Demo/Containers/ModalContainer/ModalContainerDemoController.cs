using Haondt.Web.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Haondt.Web.UI.Demo.Containers.ModalContainer
{
    [Route("/demo/modal-container/")]
    public class ModalContainerDemoController : UIController
    {
        [HttpGet]
        public Task<IResult> Get() => ComponentFactory.RenderComponentAsync<ModalContainerDemo>();

        [HttpGet("modal")]
        public Task<IResult> Close() => ComponentFactory.RenderComponentAsync<ModalPanelDemo>();
    }
}
