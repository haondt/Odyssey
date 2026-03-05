namespace Odyssey.GrainInterfaces.Testing
{
    public interface IDeactivatableGrain
    {
        /// <summary>
        /// Used for testing.
        /// </summary>
        /// <returns></returns>
        Task DeactivateOnIdleAsync();
    }
}
