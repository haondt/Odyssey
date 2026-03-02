using Haondt.Web.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Odyssey.UI.Fragments.Attributes
{
    public class FragmentsExceptionFilter(ILogger<FragmentsExceptionFilter> logger) : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, "Uncaught exception occurred while rendering fragment.");

            var response = context.HttpContext.Response.AsResponseData();
            response.Status(500)
                .HxReswap("none");

            context.Result = new ObjectResult("Uncaught exception occurred while rending fragment.");
        }

    }
}
