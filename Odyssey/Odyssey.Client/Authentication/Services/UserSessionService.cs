using Haondt.Core.Models;
using Microsoft.AspNetCore.Identity;
using Odyssey.Core.Constants;
using Odyssey.Persistence;
using Odyssey.Persistence.Models;
using System.Security.Claims;

namespace Odyssey.Client.Authentication.Services
{
    public class UserSessionService(
        ApplicationDbContext dbContext,
        SignInManager<UserDataSurrogate> signInManager,
        UserManager<UserDataSurrogate> userManager) : IUserSessionService
    {
        public Task<DetailedResult<UserDataSurrogate, List<IdentityError>>> RegisterAdministratorAsync(string username, string password) =>
            RegisterUserAsync(username, password, [AuthConstants.AdminRole]);

        public async Task<DetailedResult<UserDataSurrogate, List<IdentityError>>> RegisterUserAsync(string username, string password, IEnumerable<string>? additionalRoles = null)
        {
            using var tx = await dbContext.Database.BeginTransactionAsync();
            var user = new UserDataSurrogate
            {
                UserName = username.Trim(),
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return new(result.Errors.ToList());

            if (additionalRoles is not null)
                foreach (var additionalRole in additionalRoles)
                {
                    result = await userManager.AddToRoleAsync(user, additionalRole);
                    if (!result.Succeeded)
                        return new(result.Errors.ToList());
                }

            await tx.CommitAsync();

            return new(user);
        }

        public Task SignOutAsync()
        {
            return signInManager.SignOutAsync();
        }

        public async Task<DetailedResult<string>> TrySignInAsync(string username, string password)
        {
            var result = await signInManager.PasswordSignInAsync(username, password, true, false);
            if (!result.Succeeded)
                return new("Incorrect username or password");
            return new();
        }

        public async Task<Result<UserDataSurrogate>> GetUserAsync(ClaimsPrincipal claims)
        {
            var surrogate = await userManager.GetUserAsync(claims);
            if (surrogate == null)
                return new();
            return surrogate;
        }

    }
}
