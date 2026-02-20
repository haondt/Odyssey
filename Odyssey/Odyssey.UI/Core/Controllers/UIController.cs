using Haondt.Web.Core.Extensions;
using Haondt.Web.Core.Http;
using Haondt.Web.UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.UI.Core.Components;

namespace Odyssey.UI.Core.Controllers
{

    [ServiceFilter(typeof(ModelStateValidationFilter))]
    [Authorize]
    public class UIController : Haondt.Web.Core.Controllers.UIController
    {
        [FromServices]
        public ModelStateValidationFilter ModelStateValidationFilter { get; set; } = default!;

        protected IResponseData ResponseData
        {
            get => field ??= HttpContext.Response.AsResponseData();
        }


        protected async Task<IResult> RenderValidationComponent(Dictionary<string, string> errors)
        {
            var result = await ModelStateValidationFilter.ApplyValidationErrorAsync(errors, HttpContext);
            if (result.IsSuccessful)
                return result.Value;

            throw new InvalidOperationException("Method does not have validation state attribute.");
        }
        protected Task<IResult> Render404NotFoundComponent() => ComponentFactory.RenderComponentAsync(new ErrorPage
        {
            StatusCode = 404,
            Message = "Not found"

        });
    }
}
