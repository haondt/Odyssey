using Haondt.Core.Models;
using Odyssey.Persistence.Models;

namespace Odyssey.Domain.Core.Models
{
    public record BoardMetadata
    {
        public required string Name { get; set; }
        public required string GameId { get; set; }
        public required AbsoluteDateTime CreatedOn { get; init; }
        public required AbsoluteDateTime ModifiedOn { get; init; }

        public BoardMetadataDataModel AsDataModel(OwnedEntityId<Guid> id) => new()
        {
            Id = id.StringValue,
            Name = Name,
            GameId = GameId,
            EntityId = id.EntityId,
            SearchData = NormalizedString.Create(Name),
            CreatedOn = CreatedOn,
            ModifiedOn = ModifiedOn
        };

        public static BoardMetadata FromDataModel(BoardMetadataDataModel dataModel) => new()
        {
            Name = dataModel.Name,
            GameId = dataModel.GameId,
            CreatedOn = dataModel.CreatedOn,
            ModifiedOn = dataModel.ModifiedOn
        };
    }
}
