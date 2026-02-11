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


        protected async Task<IResult> RenderValidationComponent(Dictionary<string, string> errors)
        {
            var result = await ModelStateValidationFilter.ApplyValidationErrorAsync(errors, HttpContext);
            if (result.IsSuccessful)
                return result.Value;

            throw new InvalidOperationException("Method does not have validation component attribute.");
        }

        protected Task<IResult> RenderValidationSummaryComponent(string summary)
        {
            return RenderValidationComponent(new()
            {
                ["summary"] = summary
            });
        }
    }
}
