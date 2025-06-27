using System.Linq.Expressions;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Altura.BidManagement.Infrastructure.Persistence.Common.Repository;

public class Repository<T>(ApplicationDbContext dbContext, IUnitOfWork unitOfWork, ILogger<Repository<T>> logger) :
    IRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();
    
    public async Task<Result<T>> AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Ok(entity);
    }

    public async Task<Result<T>> UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        try
        {
            _dbSet.Update(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok(entity);
        }
        catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
        {
            // Handle concurrency exception
            logger.LogError("Concurrency error occurred while updating the entity {Message}",
                dbUpdateConcurrencyException.Message);
            return Result.Fail($"Error occurred while updating {entity.GetType().Name}.");
        }
        catch (DbUpdateException dbUpdateException)
        {
            logger.LogError("Database update error occurred while updating the entity {Message}",
                dbUpdateException.Message);
            return Result.Fail($"Update error occurred while updating {entity.GetType().Name}.");
        }
        catch (Exception ex)
        {
            logger.LogError("An unexpected error occurred while updating the entity {Message}", ex.Message);
            return Result.Fail($"An unexpected error occurred, please try again later.");
        }

    }

    public async Task<Result> DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await GetAsync(predicate, cancellationToken);
            _dbSet.Remove(entity!);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        catch (ArgumentNullException ex)
        {
            logger.LogError(ex, "Delete operation failed: {Message}", ex.Message);
            return Result.Fail("Delete operation failed.");
        }
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbSet.FindAsync(id, cancellationToken);
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }
    
    public async Task<Result<List<T>>> GetAllAsync(
        Expression<Func<T, bool>>? predicate,
        Expression<Func<T, object>>[]? includeProperties,
        CancellationToken cancellationToken)
    {
        var query = _dbSet.AsNoTracking();
        
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        
        if (includeProperties != null || includeProperties!.Length > 0)
        {
            query = includeProperties.
                Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
        
        return Result.Ok(await query.ToListAsync(cancellationToken));
    }
}