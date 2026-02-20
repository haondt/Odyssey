using Haondt.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Odyssey.UI.Core.Components;
using Odyssey.UI.Core.Models;

namespace Odyssey.UI.Core.Middlewares
{
    public class CatchAllErrorPageExceptionActionResultFactory(
        IComponentFactory componentFactory,
        ILogger<CatchAllErrorPageExceptionActionResultFactory> logger,
        IOptions<UISettings> uiSettings) : ITargetedExceptionActionResultFactory
    {
        public bool CanHandle(Exception exception) => true;

        public async Task<IResult> CreateAsync(Exception exception, HttpContext context)
        {
            logger.LogError(exception, "Uncaught error occurred.");

            var result = new ErrorPage
            {
                StatusCode = 500,
                Message = "An unexpected error occurred"
            };

            if (uiSettings.Value.ShowDetailedCatchAllExceptions)
            {
                result.Message = exception.Message;
                result.InternalDetails = exception.ToString();
            }

            var errorComponent = await componentFactory.RenderComponentAsync(result);
            return errorComponent;
        }
    }
}
