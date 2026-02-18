using Haondt.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Odyssey.Persistence.Models
{
    public record BoardMetadataDataModel
    {
        public Guid Id { get; set; } = default!;
        public required string GameId { get; set; }

        public UserDataSurrogate Owner { get; set; } = default!;
        public string OwnerId { get; set; } = default!;

        public required string Name { get; set; }
        public required NormalizedString SearchData { get; set; }
        public required AbsoluteDateTime CreatedOn { get; set; }
        public required AbsoluteDateTime ModifiedOn { get; set; }
    }

    public class BoardMetadataDataModelConfiguration : IEntityTypeConfiguration<BoardMetadataDataModel>
    {
        public void Configure(EntityTypeBuilder<BoardMetadataDataModel> builder)
        {
            builder.HasOne(x => x.Owner)
                .WithMany(r => r.BoardMetadatas)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
