using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Odyssey.Core.Constants;
using Odyssey.Domain.Authentication.Services;
using Odyssey.Domain.Core.Models;
using Odyssey.Persistence;
using Odyssey.Persistence.Models;

namespace Odyssey.Domain.Core.Services;

public class DbSeeder(
    UserManager<UserDataSurrogate> userManager,
    RoleManager<IdentityRole> roleManager,
    IUserService userService,
    ApplicationDbContext dbContext,
    IOptions<AdminSettings> adminSettings) : IDbSeeder
{
    public async Task SeedAsync()
    {
        foreach (var role in AuthConstants.Roles)
        {
            if (await roleManager.RoleExistsAsync(role))
                continue;
            var result = await roleManager.CreateAsync(new(role));
            if (!result.Succeeded)
                throw new InvalidOperationException($"Unable to register role \"{role}\" due to error(s): {string.Join('\n', result.Errors)}");
        }

        if (adminSettings.Value.RegisterDefaultAdminUser)
        {
            if (await userManager.FindByNameAsync(adminSettings.Value.DefaultAdminUsername!) is null)
            {
                var result = await userService.RegisterAdministratorAsync(adminSettings.Value.DefaultAdminUsername!, adminSettings.Value.DefaultAdminPassword!);
                if (!result.IsSuccessful)
                    throw new InvalidOperationException($"Unable to register default admin due to error(s): {string.Join('\n', result.Reason.Select(r => r.ToString()))}");
            }
        }
    }
}
