using Haondt.Web.Core.Extensions;
using Haondt.Web.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.UI.Core.Middlewares;
using Odyssey.UI.Library.Components.Element;

namespace Odyssey.UI.Core.Controllers
{

    [Produces("text/html")]
    [ServiceFilter(typeof(ModelStateValidationFilter))]
    public class UIController(IComponentFactory componentFactory) : Controller
    {
        protected readonly IComponentFactory _componentFactory = componentFactory;

        public Task<IResult> ToastResponse(
            string message,
            ToastSeverity severity = ToastSeverity.Error,
            int statusCode = 500)
        {
            var toast = new Toast
            {
                Message = message,
                Severity = severity
            };

            Response.AsResponseData()
                .Status(statusCode)
                .HxReswap("none");
            return _componentFactory.RenderComponentAsync(toast);
        }
    }
}
