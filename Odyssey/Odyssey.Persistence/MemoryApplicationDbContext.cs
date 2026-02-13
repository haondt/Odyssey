using Microsoft.EntityFrameworkCore;

namespace Odyssey.Persistence
{
    public class MemoryApplicationDbContext(DbContextOptions<MemoryApplicationDbContext> options) : ApplicationDbContext(options)
    {
    }
}
