using Haondt.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Core;
using Orleans.Runtime;
using Orleans.Serialization;
using Orleans.Serialization.TypeSystem;
using Orleans.Storage;

namespace Haondt.Orleans.Persistence
{
    public class RewindablePersistentState<TState>(IPersistentState<TState> persistentState, DeepCopier<TState> copier) : IRewindablePersistentState<TState> where TState : notnull
    {
        public TState State { get => persistentState.State; set => persistentState.State = value; }

        public string? Etag => persistentState.Etag;

        public bool RecordExists => persistentState.RecordExists;

        public Task ClearStateAsync() => persistentState.ClearStateAsync();

        public Task ReadStateAsync() => persistentState.ReadStateAsync();

        public async Task TryAndWriteStateAsync(Action action)
        {
            var oldState = copier.Copy(State);
            try
            {
                action();
                await WriteStateAsync();
            }
            catch
            {
                persistentState.State = oldState;
                throw;
            }
        }

        public async Task TryAndWriteStateAsync(Func<Task> action)
        {
            var oldState = copier.Copy(State);
            try
            {
                await action();
                await WriteStateAsync();
            }
            catch
            {
                persistentState.State = oldState;
                throw;
            }
        }

        public async Task<T> TryAndWriteStateAsync<T>(Func<Task<T>> action)
        {
            var oldState = copier.Copy(State);
            try
            {
                var result = await action();
                await WriteStateAsync();
                return result;
            }
            catch
            {
                persistentState.State = oldState;
                throw;
            }
        }

        public async Task<T> TryAndWriteStateAsync<T>(Func<T> action)
        {
            var oldState = copier.Copy(State);
            try
            {
                var result = action();
                await persistentState.WriteStateAsync();
                return result;
            }
            catch
            {
                persistentState.State = oldState;
                throw;
            }
        }

        public Task WriteStateAsync() => persistentState.WriteStateAsync();
    }
}

