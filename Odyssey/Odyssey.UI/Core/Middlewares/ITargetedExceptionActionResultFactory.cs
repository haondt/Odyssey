using Haondt.Web.Core.Services;
using Microsoft.AspNetCore.Http;

namespace Odyssey.UI.Core.Middlewares
{
    public interface ITargetedExceptionActionResultFactory : IExceptionActionResultFactory
    {
        public bool CanHandle(Exception exception, HttpContext context);
    }
}
