using Haondt.Web.Core.Extensions;
using Haondt.Web.Services;
using Haondt.Web.UI.Components.Element;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.UI.Core.Middlewares;

namespace Odyssey.UI.Core.Controllers
{

    [ServiceFilter(typeof(ModelStateValidationFilter))]
    public class UIController(IComponentFactory componentFactory) : Haondt.Web.Core.Controllers.UIController
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
