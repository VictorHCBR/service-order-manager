using ServiceOrders.Application.Abstractions;
using ServiceOrders.Application.Common;
using ServiceOrders.Domain.Common;
using ServiceOrders.Domain.Entities.ServiceOrders;

namespace ServiceOrders.Application.ServiceOrders.Queries;

public class ListServiceOrdersQueryHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<PagedResult<ServiceOrderSummaryDto>> HandleAsync(
        ServiceOrderStatus? status,
        Guid? technicianId,
        int page,
        int pageSize,
        CancellationToken token)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var all = await _unitOfWork.ServiceOrders.GetAsync(o =>
            (status == null || o.Status == status) &&
            (technicianId == null || o.AssignedTechnicianId == technicianId),
            token);

        if (_currentUser.IsInRole("Technician") && !_currentUser.IsInRole("Manager") && !_currentUser.IsInRole("Admin"))
            all = [.. all.Where(o => o.AssignedTechnicianId == _currentUser.UserId)];

        var total = all.Count;
        var items = all
            .OrderByDescending(x => x.Priority)
            .ThenBy(x => x.DueDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => x.ToSummaryDto())
            .ToList();

        return new PagedResult<ServiceOrderSummaryDto>(items, page, pageSize, total);
    }
}


public sealed class GetServiceOrderByIdQueryHandler(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<ServiceOrderDetailsDto>> HandleAsync(Guid id, CancellationToken token)
    {
        var order = await _unitOfWork.ServiceOrders.GetByIdAsync(id, token);

        return order is null ?
            Result<ServiceOrderDetailsDto>.Failure("OS não encontrada.") :
            Result<ServiceOrderDetailsDto>.Success(order.ToDetailsDto());
    }
}