using Haondt.Web.Core.Services;

namespace Odyssey.UI.Core.Middlewares
{
    public interface ITargetedExceptionActionResultFactory : IExceptionActionResultFactory
    {
        public bool CanHandle(Exception exception);
    }
}
