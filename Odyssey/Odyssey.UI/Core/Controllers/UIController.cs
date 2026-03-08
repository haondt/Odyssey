using Haondt.Web.Core.Extensions;
using Haondt.Web.Core.Http;
using Haondt.Web.UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.UI.Core.Attributes;

namespace Odyssey.UI.Core.Controllers
{

    [Authorize]
    [ServiceFilter(typeof(ModelStateValidationFilter), Order = 50)]
    [HxVaryHeader(Order = 10)]
    [EnrichUIRequestContext(Order = 10)]
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

            return await ModelStateValidationFilter.ApplyValidationComponentFromAttributeAsync(HttpContext);
        }
    }
}
