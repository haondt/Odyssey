using Haondt.Core.Extensions;
using Haondt.Core.Models;
using Odyssey.Persistence.Models;

namespace Odyssey.Domain.Core.Models
{
    public record SessionMetadata
    {
        public required string Name { get; set; }
        public required string GameId { get; set; }
        public required Guid BoardId { get; set; }
        public required string BoardName { get; set; }
        public required AbsoluteDateTime CreatedOn { get; init; }
        public Optional<AbsoluteDateTime> LastPlayedOn { get; init; }
        public SessionStatus Status { get; set; } = SessionStatus.Created;

        public SessionMetadataDataModel AsDataModel(OwnedEntityGuid id) => new()
        {
            Id = id,
            Name = Name,
            GameId = GameId,
            EntityId = id.EntityId,
            BoardEntityId = BoardId,
            BoardName = BoardName,
            BoardId = new OwnedEntityGuid(id.OwnerId, BoardId),
            SearchData = SessionMetadataDataModel.CreateSearchData(Name, BoardName),
            CreatedOn = CreatedOn,
            LastPlayedOn = LastPlayedOn.Unwrap()
        };

        public static SessionMetadata FromDataModel(SessionMetadataDataModel dataModel) => new()
        {
            Name = dataModel.Name,
            GameId = dataModel.GameId,
            BoardId = dataModel.BoardEntityId,
            BoardName = dataModel.BoardName,
            CreatedOn = dataModel.CreatedOn,
            LastPlayedOn = dataModel.LastPlayedOn.AsOptional(),
        };
    }
}
