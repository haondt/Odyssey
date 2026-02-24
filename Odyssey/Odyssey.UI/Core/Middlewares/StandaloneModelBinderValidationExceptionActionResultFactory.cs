using Haondt.Web.Core.Extensions;
using Haondt.Web.UI.Filters;
using Microsoft.AspNetCore.Http;
using Odyssey.Client.Core.Exceptions;
using Odyssey.UI.Core.Attributes;

namespace Odyssey.UI.Core.Middlewares
{
    public class StandaloneModelBinderValidationExceptionActionResultFactory : ITargetedExceptionActionResultFactory
    {
        public bool CanHandle(Exception exception, HttpContext context)
        {
            if (exception is not StandaloneModelBinderValidationException)
                return false;

            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata.GetMetadata<StandaloneModelValidationAttribute>() == null)
                return false;
            return true;
        }

        public Task<IResult> CreateAsync(Exception exception, HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata.GetMetadata<StandaloneModelValidationAttribute>() is not { } attribute)
                throw new InvalidOperationException($"Endpoint {endpoint} is missing {nameof(StandaloneModelValidationAttribute)}");

            var binderException = (StandaloneModelBinderValidationException)exception;
            var response = context.Response.AsResponseData()
                .Status(400);

            ModelStateValidationFilter.SetValidationState(context, binderException.Model);
            return ModelStateValidationFilter.ApplyValidationComponentAsync(binderException.ComponentType, context, binderException.HxSwapId, attribute.ShowToast);
        }
    }
}
