namespace Odyssey.GrainInterfaces.Core.Models
{
    public class DataStorageModel<TData> where TData : class, new()
    {
        public DataStorageModel()
        {
            Data = new();
            Version = 0;
        }

        public required TData Data { get; set; }
        public required int Version { get; set; }
    }
}
