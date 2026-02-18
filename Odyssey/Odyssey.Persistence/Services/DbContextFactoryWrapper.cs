using Microsoft.EntityFrameworkCore;

namespace Odyssey.Persistence.Services
{
    public class DbContextFactoryWrapper<TSource, TDestination>(IDbContextFactory<TSource> sourceFactory) : IDbContextFactory<TDestination> where TSource : TDestination where TDestination : DbContext
    {
        public TDestination CreateDbContext() => sourceFactory.CreateDbContext();
    }
}
