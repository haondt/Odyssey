using Haondt.Core.Models;
using Newtonsoft.Json;
using Odyssey.Domain.Core.Models;

namespace Odyssey.UI.Host.Controllers
{
    public partial class HostController
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
        }
    }
}
