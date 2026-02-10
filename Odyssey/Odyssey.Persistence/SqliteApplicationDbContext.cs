using Microsoft.EntityFrameworkCore;

namespace Odyssey.Persistence
{
    public class SqliteApplicationDbContext(DbContextOptions<SqliteApplicationDbContext> options) : ApplicationDbContext(options)
    {
    }
}
