using Haondt.Core.Models;
using Haondt.Web.Components;
using Haondt.Web.Core.Extensions;
using Haondt.Web.Services;
using Haondt.Web.UI.Attributes;
using Haondt.Web.UI.Components.Element;
using Haondt.Web.UI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

namespace Haondt.Web.UI.Filters
{
    public class ModelStateValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid)
            {
                await next();
                return;
            }

            if (TryGetValidationStateAttribute(context.HttpContext).TryGetValue(out var attr))
            {
                SetValidationState(context.HttpContext, context.ModelState);
                var result = await ApplyValidationComponentFromAttributeAsync(context.HttpContext, attr);
                await result.ExecuteAsync(context.HttpContext);
                return;
            }

            await next();
        }

        public static void SetValidationState(HttpContext httpContext, ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
                return;

            var errors = modelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(x => x.Key, x => string.Join('\n', x.Value!.Errors.Select(e => e.ErrorMessage)));

            SetValidationState(httpContext, errors);
        }

        public static void SetValidationState(HttpContext httpContext, Dictionary<string, string> validationErrors)
        {
            var validationState = httpContext.RequestServices.GetRequiredService<IValidationStateWriter>();
            validationState.ValidationErrors = validationErrors;
            validationState.ValidationSummary = string.Join('\n', validationErrors.Values);
            validationState.IsValidation = true;
        }

        private Result<ValidationStateAttribute> TryGetValidationStateAttribute(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata.GetMetadata<ValidationStateAttribute>() is { } attr)
                return new(attr);
            return new();
        }

        public Task<IResult> ApplyValidationComponentAsync<TValidationComponent>(HttpContext httpContext, Optional<string> hxSwapId = default) where TValidationComponent : IComponent
        {
            return ApplyValidationComponentAsync(typeof(TValidationComponent), httpContext, hxSwapId);
        }

        public Task<IResult> ApplyValidationComponentFromAttributeAsync(HttpContext httpContext, ValidationStateAttribute? validationState = null)
        {
            if (validationState is not { } attr)
                if (!TryGetValidationStateAttribute(httpContext).TryGetValue(out attr))
                    throw new InvalidOperationException($"Endpoint {httpContext.GetEndpoint()} is missing {nameof(ValidatableTypeAttribute)}");

            return ApplyValidationComponentAsync(attr.ComponentType, httpContext, attr.HxSwapId);
        }

        public static async Task<IResult> ApplyValidationComponentAsync(Type validationComponentType, HttpContext httpContext, Optional<string> hxSwapId = default, bool showToast = false)
        {
            var endpoint = httpContext.GetEndpoint();

            var instance = ActivatorUtilities.CreateInstance(httpContext.RequestServices, validationComponentType);
            if (instance is not IComponent component)
                throw new InvalidOperationException($"{validationComponentType.Name} must implement {nameof(IComponent)}.");
            var componentType = validationComponentType;
            if (showToast)
            {
                var toast = new Toast
                {
                    Severity = ToastSeverity.Error,
                    Text = "Operation failed, check the form for errors"
                };
                component = new AppendComponentLayout
                {
                    Components = [toast, component]
                };
                componentType = typeof(AppendComponentLayout);
            }

            var componentFactory = httpContext.RequestServices.GetRequiredService<IComponentFactory>();
            var responseData = httpContext.Response.AsResponseData();
            var result = await componentFactory.RenderComponentAsync(component, componentType);
            if (hxSwapId.TryGetValue(out var swapId))
            {
                responseData.HxReswap("morph:outerHTML");
                responseData.HxRetarget($"#{swapId}");
            }
            else
            {
                responseData.HxReswap("none");
            }
            responseData.Status(400);

            return result;
        }
    }
}
