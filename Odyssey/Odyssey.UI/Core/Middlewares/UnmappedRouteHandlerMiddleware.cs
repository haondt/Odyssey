using Haondt.Web.Core.Extensions;
using Haondt.Web.Services;
using Haondt.Web.UI.Components.Element;
using Microsoft.AspNetCore.Http;
using Odyssey.UI.Core.Components;
using Odyssey.UI.Core.Extensions;

namespace Odyssey.UI.Core.Middlewares
{
    public class UnmappedRouteHandlerMiddleware(RequestDelegate next, IComponentFactory componentFactory)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            await next(context);

            if (context.Response.StatusCode != StatusCodes.Status404NotFound)
                return;

            if (context.Response.ContentLength != null)
                return;

            if (!string.IsNullOrEmpty(context.Response.ContentType))
                return;

            var request = context.Request.AsRequestData();

            IResult result;
            if (!request.IsHxRequest() || request.IsHxBoosted())
            {
                var component = new ErrorPage
                {
                    StatusCode = 404,
                    Message = "Not found"
                };

                result = await componentFactory.RenderComponentAsync(component);
            }
            else
            {
                var component = new Toast
                {
                    Text = $"The requested fragment at {context.Request.Path} could not be found.",
                    Severity = ToastSeverity.Error,
                    StatusCode = 500
                };
                result = await componentFactory.RenderComponentAsync(component);

                var response = context.Response.AsResponseData();
                response.HxReswap("none");
            }

            await result.ExecuteAsync(context);
        }
    }
}
