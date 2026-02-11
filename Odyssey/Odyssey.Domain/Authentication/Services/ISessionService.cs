using Haondt.Core.Models;

namespace Odyssey.Domain.Authentication.Services
{
    public interface ISessionService
    {
        public bool IsAuthenticated { get; }

        Task<Result<string>> GetUsernameAsync();
    }
}
