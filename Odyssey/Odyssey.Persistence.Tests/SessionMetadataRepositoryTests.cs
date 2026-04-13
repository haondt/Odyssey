using FluentAssertions;
using Haondt.Core.Models;
using Microsoft.EntityFrameworkCore;
using Odyssey.Core.Models;
using Odyssey.Domain.Core.Services;
using Odyssey.Persistence.Models;
using Odyssey.Persistence.Tests.Fixtures;
using Odyssey.Tests.Core.Fakes;

namespace Odyssey.Persistence.Tests
{

    public class SessionMetadataRepositoryTests : IClassFixture<PostgresDbContextFixture>
    {
        private readonly PostgresApplicationDbContextFactory _dbContext;
        private class TestContext(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            public required SessionMetadataRepository Sut { get; init; }
            public required FakeClock Clock { get; init; }

            public AbsoluteDateTime LockClock()
            {
                var now = AbsoluteDateTime.Now;
                Clock.NowFactory = () => now;
                return now;
            }

            public async Task<string> CreateUserAsync()
            {
                var userId = Guid.NewGuid().ToString("N");
                await using var ctx = await dbContextFactory.CreateDbContextAsync();
                ctx.Users.Add(new()
                {
                    Id = userId,
                    UserName = userId,
                    NormalizedUserName = NormalizedString.Create(userId),
                    SecurityStamp = Guid.NewGuid().ToString(),
                });
                await ctx.SaveChangesAsync();
                return userId;
            }

            public async Task<(Guid, BoardMetadataDataModel)> CreateBoardAsync(string? ownerId = default, string? gameId = default)
            {
                gameId ??= Guid.NewGuid().ToString("N");
                ownerId ??= await CreateUserAsync();
                var boardId = Guid.NewGuid();
                await using var ctx = await dbContextFactory.CreateDbContextAsync();
                var board = ctx.BoardMetadatas.Add(new()
                {
                    Id = new OwnedEntityGuid(ownerId, boardId),
                    CreatedOn = Clock.Now,
                    EntityId = boardId,
                    GameId = gameId,
                    ModifiedOn = Clock.Now,
                    Name = Guid.NewGuid().ToString("N"),
                    OwnerId = ownerId,
                    SearchData = Guid.NewGuid().ToString("N")
                });
                await ctx.SaveChangesAsync();
                return (board.Entity.EntityId, board.Entity);
            }

            public async Task<int> DeleteBoardAsync(OwnedEntityGuid id)
            {
                await using var ctx = await dbContextFactory.CreateDbContextAsync();
                return await ctx.BoardMetadatas
                    .Where(b => b.Id == id)
                    .ExecuteDeleteAsync();
            }
        }

        public SessionMetadataRepositoryTests(PostgresDbContextFixture dbFixture)
        {
            _dbContext = dbFixture.Factory;
        }

        private TestContext CreateTestContext()
        {
            var clock = new FakeClock();
            return new(_dbContext)
            {
                Sut = new(_dbContext, clock),
                Clock = clock
            };
        }


        [Fact]
        public async Task CanCreateAndRetrieveSessionMetadataAsync()
        {
            var ctx = CreateTestContext();
            var now = ctx.LockClock();
            var sessionName = "My Session";
            var (boardId, board) = await ctx.CreateBoardAsync();
            var (id, writeSession) = await ctx.Sut.CreateSessionMetadataAsync(board.GameId, board.OwnerId, sessionName, boardId, board.Name, false);

            writeSession.Name.Should().BeEquivalentTo(sessionName);
            writeSession.GameId.Should().BeEquivalentTo(board.GameId);
            writeSession.BoardId.Should().Be(boardId);
            writeSession.BoardName.Should().BeEquivalentTo(board.Name);
            writeSession.CreatedOn.Should().Be(now);
            writeSession.Phase.Should().Be(Models.SessionPhase.Created);

            var readSessionResult = await ctx.Sut.GetSessionMetadataAsync((board.OwnerId, id));
            var readSession = readSessionResult.Value!;
            readSession.Name.Should().BeEquivalentTo(sessionName);
            readSession.GameId.Should().BeEquivalentTo(board.GameId);
            readSession.BoardId.Should().Be(boardId);
            readSession.BoardName.Should().BeEquivalentTo(board.Name);
            readSession.CreatedOn.Should().Be(now);
            readSession.Phase.Should().Be(Models.SessionPhase.Created);
        }

        [Fact]
        public async Task DeletingBoardWillNotDeleteSession()
        {
            var ctx = CreateTestContext();
            var now = ctx.LockClock();
            var sessionName = "My Session";
            var (boardId, board) = await ctx.CreateBoardAsync();
            var (id, _) = await ctx.Sut.CreateSessionMetadataAsync(board.GameId, board.OwnerId, sessionName, boardId, board.Name, false);
            var result = await ctx.DeleteBoardAsync((board.OwnerId, boardId));
            result.Should().Be(1);

            var readSessionResult = await ctx.Sut.GetSessionMetadataAsync((board.OwnerId, id));
            var readSession = readSessionResult.Value!;
            readSession.Name.Should().BeEquivalentTo(sessionName);
            readSession.GameId.Should().BeEquivalentTo(board.GameId);
            readSession.BoardId.Should().Be(boardId);
            readSession.BoardName.Should().BeEquivalentTo(board.Name);
            readSession.CreatedOn.Should().Be(now);
            readSession.Phase.Should().Be(Models.SessionPhase.Created);
        }
    }
}
