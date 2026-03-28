using Orleans.Runtime;

namespace Haondt.Orleans.Persistence
{
    public interface IRewindablePersistentStateFactory
    {
        IRewindablePersistentState<TState> Create<TState>(IGrainContext context, string stateName, string storageName) where TState : notnull;

        IRewindablePersistentState<TState> Create<TState>(IGrainContext context, IPersistentStateConfiguration config) where TState : notnull;
    }
}
