using Haondt.Web.Services;
using Microsoft.AspNetCore.Http;
using Odyssey.UI.Core.Components;

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

            var component = new ErrorPage
            {
                StatusCode = 404,
                Message = "Not found"
            };

            var result = await componentFactory.RenderComponentAsync(component);
            await result.ExecuteAsync(context);
        }
    }
}
