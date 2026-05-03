using ServiceOrders.Domain.Entities.ServiceOrders;
using ServiceOrders.Domain.Entities.Users;

namespace ServiceOrders.Application.Abstractions;

public interface IUnitOfWork
{
    IRepository<User> Users { get; }
    IRepository<ServiceOrder> ServiceOrders { get; }
    Task<int> SaveChangesAsync(CancellationToken token = default);
}
