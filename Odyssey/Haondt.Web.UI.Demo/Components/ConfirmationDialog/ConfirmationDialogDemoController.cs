using Haondt.Web.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Haondt.Web.UI.Demo.Components.ConfirmationDialog
{
    [Route("/demo/confirmation-dialog/")]
    public class ConfirmationDialogDemoController : UIController
    {
        [HttpGet("")]
        public Task<IResult> Get() => ComponentFactory.RenderComponentAsync<ConfirmationDialogDemo>();

        [HttpGet("show")]
        public Task<IResult> Show()
        {
            var model = new UI.Components.Components.ConfirmationDialog
            {
                Title = "Confirm action",
                Message = "Are you sure you want to proceed?",
                Intent = Haondt.Web.UI.Components.Components.ConfirmationDialogIntent.Modify,
                ConfirmText = "Confirm",
                CancelText = "Cancel"
            };
            return ComponentFactory.RenderComponentAsync(model);
        }
    }
}
