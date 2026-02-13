using Haondt.Core.Models;
using Microsoft.AspNetCore.Identity;
using Odyssey.Persistence.Models;
using System.Security.Claims;

namespace Odyssey.Client.Authentication.Services
{
    public interface IUserSessionService
    {
        Task<Result<UserDataSurrogate>> GetUserAsync(ClaimsPrincipal claims);
        Task<DetailedResult<UserDataSurrogate, List<IdentityError>>> RegisterAdministratorAsync(string username, string password);
        Task<DetailedResult<UserDataSurrogate, List<IdentityError>>> RegisterUserAsync(string username, string password, IEnumerable<string>? roles = null);

        Task SignOutAsync();
        Task<DetailedResult<string>> TrySignInAsync(string username, string password);
    }
}
