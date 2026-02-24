using Haondt.Web.Core.Extensions;
using Haondt.Web.Services;
using Haondt.Web.UI.Components.Element;
using Microsoft.AspNetCore.Http;
using Odyssey.UI.Core.Exceptions;

namespace Odyssey.UI.Core.Middlewares
{
    public class ToastExceptionActionResultFactory(IComponentFactory componentFactory) : ITargetedExceptionActionResultFactory
    {
        public bool CanHandle(Exception exception, HttpContext context) => exception is ToastException;

        public async Task<IResult> CreateAsync(Exception exception, HttpContext context)
        {
            var toastException = (ToastException)exception;
            var result = new Toast
            {
                Title = toastException.Title,
                StatusCode = toastException.StatusCode,
                Text = toastException.Message,
                Severity = toastException.Severity,
            };

            var errorComponent = await componentFactory.RenderComponentAsync(result);

            context.Response.AsResponseData().HxReswap("none");
            return errorComponent;
        }
    }
}
