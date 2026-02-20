using Haondt.Core.Models;

namespace Odyssey.Client.Authentication.Services
{
    public interface ISessionService
    {
        public bool IsAuthenticated { get; }

        Task<string> GetUserIdAsync();
        Task<Result<string>> GetUserNameAsync();
    }
}
