using Haondt.Core.Models;
using Odyssey.Persistence.Models;

namespace Odyssey.Domain.Core.Models
{
    public record BoardMetadata
    {
        public required string Name { get; set; }
        public required string GameId { get; set; }
        public required AbsoluteDateTime CreatedOn { get; set; }
        public required AbsoluteDateTime ModifiedOn { get; set; }

        public BoardMetadataDataModel AsDataModel() => new()
        {
            GameId = GameId,
            Name = Name,
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
