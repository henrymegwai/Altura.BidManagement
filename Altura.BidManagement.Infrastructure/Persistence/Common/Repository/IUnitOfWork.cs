namespace Altura.BidManagement.Infrastructure.Persistence.Common.Repository;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}