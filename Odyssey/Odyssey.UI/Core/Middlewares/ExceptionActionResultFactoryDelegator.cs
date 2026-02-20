using Haondt.Web.Core.Services;
using Microsoft.AspNetCore.Http;

namespace Odyssey.UI.Core.Middlewares
{
    public class ExceptionActionResultFactoryDelegator(IEnumerable<ITargetedExceptionActionResultFactory> factories) : IExceptionActionResultFactory
    {
        public Task<IResult> CreateAsync(Exception exception, HttpContext context)
        {
            foreach (var factory in factories)
                if (factory.CanHandle(exception))
                    return factory.CreateAsync(exception, context);
            throw new NotSupportedException($"Cannot handle exception of type {exception.GetType()}");
        }
    }
}
