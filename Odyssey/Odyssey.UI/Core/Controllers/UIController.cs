using Haondt.Web.Core.Extensions;
using Haondt.Web.Core.Http;
using Haondt.Web.UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        protected IRequestData RequestData
        {
            get => field ??= HttpContext.Request.AsRequestData();
        }


        protected async Task<IResult> RenderValidationComponent(Dictionary<string, string> errors)
        {
            ModelStateValidationFilter.SetValidationState(HttpContext, errors);

            return await ModelStateValidationFilter.ApplyValidationComponentAsync(HttpContext);
        }
    }
}
