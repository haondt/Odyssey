using Haondt.Core.Models;
using Odyssey.Core.Models;
using Odyssey.GrainInterfaces.Core;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions.Models;

namespace Odyssey.GrainInterfaces.Sessions
{
    public interface ISessionGrain : IGrain<OwnedEntityGuid>, IGrainWithGuidCompoundKey
    {
    }

    public interface ISessionGrain<TBoard, TGameState> : ISessionGrain
        where TBoard : IDataStorageData<TBoard>
        where TGameState : IDataStorageData<TGameState>
    {
        Task ClearGameStateAsync();
        Task ClearStateAsync();
        Task<TGameState> GetGameStateAsync();
        Task<ReadOnlySessionState> GetStateAsync();
        /// <summary>
        /// Set the game state and allow it to be persisted later.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        Task SetGameStateAsync(TGameState state);
        Task SetState(int version, Optional<TBoard> board = default, Optional<List<SessionPlayer>> players = default);
        /// <summary>
        /// Explicitly write and flush the game state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        Task WriteGameStateAsync(TGameState state);
    }
}
