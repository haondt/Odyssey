using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Runtime;
using Orleans.Serialization;
using Orleans.Storage;

namespace Haondt.Orleans.Persistence
{
    public class RewindablePersistentStateFactory(IPersistentStateFactory persistentStateFactory) : IRewindablePersistentStateFactory
    {
        private record PersistentStateConfiguration(string StateName, string StorageName) : IPersistentStateConfiguration { }

        public IRewindablePersistentState<TState> Create<TState>(IGrainContext context, string stateName, string storageName) where TState : notnull
            => Create<TState>(context, new PersistentStateConfiguration(stateName, storageName));

        public IRewindablePersistentState<TState> Create<TState>(IGrainContext context, IPersistentStateConfiguration config) where TState : notnull
        {
            var persistentState = persistentStateFactory.Create<TState>(context, config);
            var copier = context.ActivationServices.GetRequiredService<DeepCopier<TState>>();
            return new RewindablePersistentState<TState>(persistentState, copier);
        }
    }

}
