using Haondt.Core.Models;
using Odyssey.Core.Models;
using Odyssey.GrainInterfaces.Core;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions.Models;

namespace Odyssey.GrainInterfaces.Sessions
{
    public interface ISessionGrain<TBoard, TGameState> : IGrain<OwnedEntityGuid>, IGrainWithGuidCompoundKey
        where TBoard : IDataStorageData<TBoard>
        where TGameState : IDataStorageData<TGameState>
    {
        Task ClearGameStateAsync();
        Task ClearStateAsync();
        Task<TGameState> GetGameStateAsync();
        Task<ReadOnlySessionState> GetStateAsync();
        Task SetState(int version, Optional<TBoard> board = default, Optional<List<SessionPlayer>> players = default);
    }
}
