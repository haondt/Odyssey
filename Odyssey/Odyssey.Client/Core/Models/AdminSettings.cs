using Microsoft.Extensions.Options;

namespace Odyssey.Client.Core.Models
{
    public class AdminSettings
    {
        public bool RegisterDefaultAdminUser { get; set; }
        public string? DefaultAdminUsername { get; set; }
        public string? DefaultAdminPassword { get; set; }
        public static OptionsBuilder<AdminSettings> Validate(OptionsBuilder<AdminSettings> builder)
        {
            builder.Validate(o => !o.RegisterDefaultAdminUser || !string.IsNullOrEmpty(o.DefaultAdminUsername),
                "Default admin username cannot be empty.");
            builder.Validate(o => !o.RegisterDefaultAdminUser || !string.IsNullOrEmpty(o.DefaultAdminPassword),
                "Default admin password cannot be empty.");

            return builder;
        }
    }
}
