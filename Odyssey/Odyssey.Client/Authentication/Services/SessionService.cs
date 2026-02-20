using Haondt.Core.Extensions;
using Haondt.Core.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Odyssey.Client.Authentication.Services
{
    public class SessionService(IHttpContextAccessor httpContextAccessor,
        IUserSessionService userService) : ISessionService
    {
        public bool IsAuthenticated
        {
            get
            {
                return httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;
            }
        }

        public async Task<Result<string>> GetUserNameAsync()
        {
            var user = httpContextAccessor.HttpContext?.User;
            if (user == null)
                return new();
            if (user.FindFirst(ClaimTypes.Name)?.Value is string userName)
                return userName;

            var surrogate = await userService.GetUserAsync(user);
            return surrogate.AsOptional().Bind(q => q.UserName.AsOptional()).AsResult();
        }

        public async Task<string> GetUserIdAsync()
        {
            var user = httpContextAccessor.HttpContext?.User;
            if (user == null)
                throw new InvalidOperationException("Unable to retrieve user from http context");
            if (user.FindFirst(ClaimTypes.NameIdentifier)?.Value is string id)
                return id;

            var surrogateResult = await userService.GetUserAsync(user);
            if (!surrogateResult.TryGetValue(out var surrogate))
                throw new InvalidOperationException("User not found");
            return surrogate.Id;
        }
    }
}
