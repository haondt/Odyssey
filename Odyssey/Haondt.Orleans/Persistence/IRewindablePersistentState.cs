using Haondt.Core.Models;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;
namespace Haondt.Orleans.Persistence
{
    public interface IRewindablePersistentState<TState> where TState : notnull
    {
        TState State { get; }
        bool RecordExists { get; }
        Task ReadStateAsync();
        Task WriteStateAsync();
        Task ClearStateAsync();

        Task TryAndWriteStateAsync(Action action);
        Task TryAndWriteStateAsync(Func<Task> action);
        Task<T> TryAndWriteStateAsync<T>(Func<Task<T>> action);
        Task<T> TryAndWriteStateAsync<T>(Func<T> action);
    }
}
