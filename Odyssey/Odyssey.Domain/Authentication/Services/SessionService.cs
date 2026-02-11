using Haondt.Core.Extensions;
using Haondt.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Odyssey.Domain.Authentication.Services
{
    public class SessionService(IHttpContextAccessor httpContextAccessor,
        IUserService userService) : ISessionService
    {
        public bool IsAuthenticated
        {
            get
            {
                return httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;
            }
        }

        // todo: cache this among other info on user grain
        public async Task<Result<string>> GetUsernameAsync()
        {
            var user = httpContextAccessor.HttpContext?.User;
            if (user == null)
                return new();

            var surrogate = await userService.GetUserAsync(user);
            return surrogate.AsOptional().Bind(q => q.UserName.AsOptional()).AsResult();
        }
    }
}
