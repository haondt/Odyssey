using Haondt.Core.Models;
using Haondt.Web.Core.Extensions;
using Haondt.Web.Services;
using Haondt.Web.UI.Attributes;
using Haondt.Web.UI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Haondt.Web.UI.Filters
{
    public class ModelStateValidationFilter(IComponentFactory componentFactory) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid)
            {
                await next();
                return;
            }


            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(x => x.Key, x => string.Join('\n', x.Value!.Errors.Select(e => e.ErrorMessage)));

            var result = await ApplyValidationErrorAsync(errors, context.HttpContext);
            if (result.IsSuccessful)
            {
                await result.Value.ExecuteAsync(context.HttpContext);
                return;
            }

            await next();
        }

        private static void UpdateValidationState(HttpContext httpContext, Dictionary<string, string> validationErrors)
        {
            var validationState = httpContext.RequestServices.GetRequiredService<IValidationStateWriter>();
            validationState.ValidationErrors = validationErrors;
            validationState.ValidationSummary = string.Join('\n', validationErrors.Values);
            validationState.IsValidation = true;
        }

        public async Task<Result<IResult>> ApplyValidationErrorAsync(Dictionary<string, string> validationErrors, HttpContext httpContext)
        {
            var endpoint = httpContext.GetEndpoint();

            if (endpoint?.Metadata.GetMetadata<ValidationStateAttribute>() is { } errorsAttribute)
            {
                UpdateValidationState(httpContext, validationErrors);

                var instance = ActivatorUtilities.CreateInstance(httpContext.RequestServices, errorsAttribute.ComponentType);
                if (instance is not IComponent component)
                    throw new InvalidOperationException($"{errorsAttribute.ComponentType} must implement {nameof(IComponent)}.");

                var result = await componentFactory.RenderComponentAsync(component, errorsAttribute.ComponentType);
                var responseData = httpContext.Response.AsResponseData();
                if (errorsAttribute.HxSwapId.TryGetValue(out var swapId))
                {
                    responseData.HxReswap("morph:outerHTML");
                    responseData.HxRetarget($"#{swapId}");
                }
                else
                {
                    responseData.HxReswap("none");
                }
                responseData.Status(400);
                return new(result);
            }

            return new();
        }
    }
}
