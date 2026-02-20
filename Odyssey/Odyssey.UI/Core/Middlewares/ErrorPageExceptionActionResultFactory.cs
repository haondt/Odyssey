using Haondt.Web.Services;
using Microsoft.AspNetCore.Http;
using Odyssey.UI.Core.Components;
using Odyssey.UI.Core.Exceptions;

namespace Odyssey.UI.Core.Middlewares
{
    public class ErrorPageExceptionActionResultFactory(IComponentFactory componentFactory) : ITargetedExceptionActionResultFactory
    {
        public bool CanHandle(Exception exception) => exception is ErrorPageException;

        public async Task<IResult> CreateAsync(Exception exception, HttpContext context)
        {
            if (exception is not ErrorPageException errorPageException)
                throw new ArgumentException(nameof(exception));

            var result = new ErrorPage
            {
                StatusCode = errorPageException.StatusCode,
                Message = errorPageException.Message,
                Details = errorPageException.Details,
                InternalDetails = errorPageException.InternalDetails
            };

            var errorComponent = await componentFactory.RenderComponentAsync(result);
            return errorComponent;
        }
    }
}
