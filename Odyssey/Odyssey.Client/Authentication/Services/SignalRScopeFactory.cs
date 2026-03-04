using Microsoft.Extensions.DependencyInjection;
using Odyssey.Client.Authentication.Models;

namespace Odyssey.Client.Authentication.Services
{
    public class SignalRScopeFactory(IServiceScopeFactory scopeFactory) : ISignalRScopeFactory
    {
        public IServiceScope CreateScope(string userId)
        {
            var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SessionContext>();
            context.IsAuthenticated = true;
            context.UserId = userId;
            return scope;
        }
    }
}
