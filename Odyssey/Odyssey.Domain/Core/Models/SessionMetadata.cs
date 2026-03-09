using Haondt.Core.Extensions;
using Haondt.Core.Models;
using Odyssey.Persistence.Models;

namespace Odyssey.Domain.Core.Models
{
    public record SessionMetadata
    {
        public required string Name { get; set; }
        public required string GameId { get; set; }
        public Optional<OwnedEntityGuid> BoardId { get; set; }
        public required AbsoluteDateTime CreatedOn { get; init; }
        public Optional<AbsoluteDateTime> LastPlayedOn { get; init; }
        public SessionStatus Status { get; set; } = SessionStatus.Created;

        public SessionMetadataDataModel AsDataModel(OwnedEntityGuid id) => new()
        {
            Id = id,
            Name = Name,
            GameId = GameId,
            EntityId = id.EntityId,
            BoardMetadataId = BoardId.Unwrap(),
            SearchData = NormalizedString.Create(Name),
            CreatedOn = CreatedOn,
            LastPlayedOn = LastPlayedOn.Unwrap()
        };

        public static SessionMetadata FromDataModel(SessionMetadataDataModel dataModel) => new()
        {
            Name = dataModel.Name,
            GameId = dataModel.GameId,
            BoardId = dataModel.BoardMetadataId.AsOptional().Map(q => (OwnedEntityGuid)q),
            CreatedOn = dataModel.CreatedOn,
            LastPlayedOn = dataModel.LastPlayedOn.AsOptional(),
        };
    }
}
