using System.Linq.Expressions;
using FluentResults;

namespace Altura.BidManagement.Infrastructure.Persistence.Common.Repository;

public interface IRepository<T> where T : class
{
    Task<Result<T>> AddAsync(T entity, CancellationToken cancellationToken);
    
    Task<Result<T>> UpdateAsync(T entity, CancellationToken cancellationToken);
    
    Task<Result> DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

    Task<Result<List<T>>> GetAllAsync(
        Expression<Func<T, bool>>? predicate,
        Expression<Func<T, object>>[]? includeProperties,
        CancellationToken cancellationToken);
}