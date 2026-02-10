using Microsoft.EntityFrameworkCore;

namespace Odyssey.Persistence
{
    public class PostgresApplicationDbContext(DbContextOptions<PostgresApplicationDbContext> options) : ApplicationDbContext(options)
    {
    }
}
