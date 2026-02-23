using Haondt.Web.Core.Extensions;
using Haondt.Web.UI.Filters;
using Microsoft.AspNetCore.Http;
using Odyssey.Client.Core.Exceptions;

namespace Odyssey.UI.Core.Middlewares
{
    public class StandaloneModelBinderValidationExceptionActionResultFactory : ITargetedExceptionActionResultFactory
    {
        public bool CanHandle(Exception exception) => exception is StandaloneModelBinderValidationException;

        public Task<IResult> CreateAsync(Exception exception, HttpContext context)
        {
            var binderException = (StandaloneModelBinderValidationException)exception;
            var response = context.Response.AsResponseData()
                .Status(400);

            ModelStateValidationFilter.SetValidationState(context, binderException.Model);
            return ModelStateValidationFilter.ApplyValidationComponentAsync(binderException.ComponentType, context, binderException.HxSwapId);
        }
    }
}
