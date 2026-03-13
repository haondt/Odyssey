using Haondt.Core.Extensions;
using Haondt.Core.Models;
using Newtonsoft.Json;
using Odyssey.Domain.Core.Models;

namespace Odyssey.UI.Core.Models
{
    public class TemporalContinuationData<TId> where TId : notnull
    {
        public TId? Id { get; set; } = default;
        public AbsoluteDateTime? Time { get; set; } = default;
        [JsonIgnore]
        public PaginationOptions<(TId, AbsoluteDateTime)> Pagination
        {
            get
            {
                var last = new Optional<(TId, AbsoluteDateTime)>();
                if (Id != null && Time.HasValue)
                    last = (Id, Time.Value);
                return new(last);
            }
        }
        public PaginationOptions<(TId Id, Optional<AbsoluteDateTime> Time)> PaginationOptionalTime
        {
            get
            {
                var last = new Optional<(TId, Optional<AbsoluteDateTime>)>();
                if (Id != null)
                    last = (Id, Time.AsOptional());
                return new(last);
            }
        }
    }
}
