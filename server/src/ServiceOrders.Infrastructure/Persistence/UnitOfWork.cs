using ServiceOrders.Application.Abstractions;
using ServiceOrders.Domain.Entities.ServiceOrders;
using ServiceOrders.Domain.Entities.Users;
using ServiceOrders.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceOrders.Infrastructure.Persistence;

public sealed class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    private readonly AppDbContext _dbContext = dbContext;

    private IRepository<User>? _users;
    private IRepository<ServiceOrder>? _serviceOrders;

    public IRepository<User> Users => _users ??= new Repository<User>(_dbContext);
    public IRepository<ServiceOrder> ServiceOrders => _serviceOrders ??= new Repository<ServiceOrder>(_dbContext);

    public Task<int> SaveChangesAsync(CancellationToken token = default)
        => _dbContext.SaveChangesAsync(token);
    
}
