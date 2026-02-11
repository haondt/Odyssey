using Haondt.Core.Models;
using Microsoft.AspNetCore.Identity;
using Odyssey.Persistence.Models;

namespace Odyssey.Domain.Core.Services
{
    public interface IUserService
    {
        Task<DetailedResult<UserDataSurrogate, List<IdentityError>>> RegisterAdministratorAsync(string username, string password);
        Task<DetailedResult<UserDataSurrogate, List<IdentityError>>> RegisterUserAsync(string username, string password, IEnumerable<string>? roles = null);
    }
}
