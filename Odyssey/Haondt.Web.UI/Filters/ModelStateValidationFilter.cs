using Haondt.Core.Models;
using Haondt.Web.Core.Extensions;
using Haondt.Web.Services;
using Haondt.Web.UI.Attributes;
using Haondt.Web.UI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Haondt.Web.UI.Filters
{
    public class ModelStateValidationFilter(IComponentFactory componentFactory, IServiceProvider serviceProvider) : IAsyncActionFilter
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

        public async Task<Result<IResult>> ApplyValidationErrorAsync(Dictionary<string, string> validationErrors, HttpContext httpContext)
        {
            var endpoint = httpContext.GetEndpoint();

            if (endpoint?.Metadata.GetMetadata<ValidationSummaryAttribute>() is { } summaryAttribute)
            {
                var text = string.Join('\n', validationErrors.Values);
                var instance = ActivatorUtilities.CreateInstance(serviceProvider, summaryAttribute.ComponentType);

                if (instance is not IValidationSummaryComponent component)
                    throw new InvalidOperationException($"{summaryAttribute.ComponentType} must implement {nameof(IValidationSummaryComponent)}.");

                component.ValidationSummary = text;
                component.IsValidation = true;


                var result = await componentFactory.RenderComponentAsync(component, summaryAttribute.ComponentType);
                var responseData = httpContext.Response.AsResponseData();
                if (summaryAttribute.HxSwapId.TryGetValue(out var swapId))
                {
                    responseData.HxReswap("outerHTML");
                    responseData.HxRetarget($"#{swapId}");
                }
                else
                {
                    responseData.HxReswap("none");
                }
                responseData.Status(400);
                return new(result);
            }

            if (endpoint?.Metadata.GetMetadata<ValidationErrorsAttribute>() is { } errorsAttribute)
            {
                var instance = ActivatorUtilities.CreateInstance(serviceProvider, errorsAttribute.ComponentType);

                if (instance is not IValidationErrorsComponent component)
                    throw new InvalidOperationException($"{errorsAttribute.ComponentType} must implement {nameof(IValidationErrorsComponent)}.");

                component.ValidationErrors = validationErrors;
                component.IsValidation = true;


                var result = await componentFactory.RenderComponentAsync(component, errorsAttribute.ComponentType);
                var responseData = httpContext.Response.AsResponseData();
                if (errorsAttribute.HxSwapId.TryGetValue(out var swapId))
                {
                    responseData.HxReswap("outerHTML");
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
