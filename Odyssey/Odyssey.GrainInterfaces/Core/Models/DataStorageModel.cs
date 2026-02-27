using Odyssey.GrainInterfaces.Core.Services;

namespace Odyssey.GrainInterfaces.Core.Models
{
    public class DataStorageModel<TData> where TData : IDataStorageData<TData>
    {
        public required TData Data { get; set; }
        public required int Version { get; set; }
    }
}
