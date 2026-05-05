using Microsoft.EntityFrameworkCore;
using ServiceOrders.Application.Abstractions;
using ServiceOrders.Domain.Common;
using ServiceOrders.Domain.Entities.Users;
using System.Linq.Expressions;

namespace ServiceOrders.Infrastructure.Persistence.Repositories;

public sealed class Repository<T>(AppDbContext dbContext) : IRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _dbContext = dbContext;

    public Task<T?> GetByIdAsync(Guid id, CancellationToken token = default)
        => ApplyDefaultIncludes(_dbContext.Set<T>()).FirstOrDefaultAsync(x => x.Id == id, token);

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken token = default)
    {
        IQueryable<T> query = ApplyDefaultIncludes(_dbContext.Set<T>());
        if (predicate is not null)
            query = query.Where(predicate);

        return await query.AsNoTracking().ToListAsync(token);
    }

    private static IQueryable<T> ApplyDefaultIncludes(IQueryable<T> query)
    {
        if (typeof(T) == typeof(User))
            return (IQueryable<T>)((IQueryable<User>)query).Include(x => x.Roles);

        return query;
    }

    public Task AddAsync(T entity, CancellationToken token = default)
        => _dbContext.Set<T>().AddAsync(entity, token).AsTask();

    public void Update(T entity) => _dbContext.Set<T>().Update(entity);

    public void Remove(T entity) => _dbContext.Set<T>().Remove(entity);
}
