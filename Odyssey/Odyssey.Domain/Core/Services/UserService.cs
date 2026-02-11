using Haondt.Core.Models;
using Microsoft.AspNetCore.Identity;
using Odyssey.Core.Constants;
using Odyssey.Persistence;
using Odyssey.Persistence.Models;

namespace Odyssey.Domain.Core.Services
{
    public class UserService(
        ApplicationDbContext dbContext,
    UserManager<UserDataSurrogate> userManager) : IUserService
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
    }
}
