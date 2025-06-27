using System.Reflection;
using Altura.BidManagement.Domain.Bids;
using Altura.BidManagement.Infrastructure.Persistence.Common.Repository;
using Microsoft.EntityFrameworkCore;

namespace Altura.BidManagement.Infrastructure.Persistence.Common
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options), IUnitOfWork
    {
        public DbSet<Bid> Bids { get; set; } = null!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
