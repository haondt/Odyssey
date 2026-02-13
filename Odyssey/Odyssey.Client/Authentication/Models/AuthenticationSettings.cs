using Microsoft.AspNetCore.Http;

namespace Odyssey.Client.Authentication.Models
{
    public class AuthenticationSettings
    {
        public required string CookieName { get; set; }
        public required bool HttpOnly { get; set; }
        public required SameSiteMode SameSite { get; set; }
        public required bool Secure { get; set; }
        public required double ExpireTimeSpanDays { get; set; }
        public required bool SlidingExpiration { get; set; }
        public List<string> Origins { get; set; } = [];
    }
}
