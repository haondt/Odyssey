using Haondt.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Odyssey.Persistence.Models
{
    public class SessionMetadataDataModel
    {
        public required string Id { get; set; }
        public required Guid EntityId { get; set; }
        public required string GameId { get; set; }
        public BoardMetadataDataModel? BoardMetadata { get; set; }
        public string? BoardMetadataId { get; set; }

        public UserDataSurrogate Owner { get; set; } = default!;
        public string OwnerId { get; set; } = default!;

        public required string Name { get; set; }
        public required string SearchData { get; set; }
        public required AbsoluteDateTime CreatedOn { get; set; }
        public required AbsoluteDateTime? LastPlayedOn { get; init; }
        public SessionStatus Status { get; set; } = SessionStatus.Created;
    }

    public class SessionMetadataDataModelConfiguration : IEntityTypeConfiguration<SessionMetadataDataModel>
    {
        public void Configure(EntityTypeBuilder<SessionMetadataDataModel> builder)
        {
            builder.HasOne(x => x.Owner)
                .WithMany(r => r.SessionMetadatas)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.BoardMetadata)
                .WithMany(r => r.SessionMetadatas)
                .HasForeignKey(x => x.BoardMetadataId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
