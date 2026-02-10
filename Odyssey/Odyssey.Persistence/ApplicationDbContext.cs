using Haondt.Core.Models;
using Haondt.Persistence.EntityFrameworkCore.Converters;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Odyssey.Persistence.Models;

namespace Odyssey.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<UserDataSurrogate>
    {

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
            var absoluteDateTimeConverter = new ValueConverter<AbsoluteDateTime, long>(
                v => v.UnixTimeSeconds,
                v => AbsoluteDateTime.Create(v));
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<AbsoluteDateTime>()
                .HaveConversion<AbsoluteDateTimeConverter>();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
