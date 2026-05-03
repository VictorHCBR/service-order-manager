using ServiceOrders.Domain.Common;
using System.Linq.Expressions;

namespace ServiceOrders.Application.Abstractions;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken token = default);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken token = default);
    Task AddAsync(T entity, CancellationToken token = default);
    void Update(T entity);
    void Remove(T entity);
}
