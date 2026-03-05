using Haondt.Web.Core.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace Odyssey.UI.Core.Attributes
{
    public sealed class HxVaryHeaderAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //context.HttpContext.Response.AsResponseData().Header().Headers.AppendCommaSeparatedValues(
            //    HeaderNames.Vary,
            //    "HX-Request",
            //    "HX-History-Restore-Request",
            //    "HX-Boosted"
            //);
            context.HttpContext.Response.AsResponseData()
                .Header(HeaderNames.Vary, "HX-Request")
                .Header(HeaderNames.Vary, "HX-History-Restore-Request")
                .Header(HeaderNames.Vary, "HX-Boosted");
        }
    }
}
