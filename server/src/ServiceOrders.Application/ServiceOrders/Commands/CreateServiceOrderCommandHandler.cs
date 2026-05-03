using ServiceOrders.Application.Abstractions;
using ServiceOrders.Domain.Common;
using ServiceOrders.Domain.Entities.ServiceOrders;
using ServiceOrders.Domain.ValueObjects;

namespace ServiceOrders.Application.ServiceOrders.Commands;

public sealed class CreateServiceOrderCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<ServiceOrderDetailsDto>> HandleAsync(CreateServiceOrderRequest request, CancellationToken token)
    {
        var todayOrders = await _unitOfWork.ServiceOrders.GetAsync(x => x.CreatedAt.Date == DateTimeOffset.UtcNow.Date, token);
        var number = ServiceOrderNumber.Generate(DateTimeOffset.UtcNow, todayOrders.Count + 1);

        var order = new ServiceOrder(
            number,
            request.Title,
            request.Description,
            request.CustomerName,
            Email.Create(request.CustomerEmail),
            request.Priority,
            _currentUser.UserId,
            request.DueDate);

        await _unitOfWork.ServiceOrders.AddAsync(order, token);
        await _unitOfWork.SaveChangesAsync(token);

        return Result<ServiceOrderDetailsDto>.Success(order.ToDetailsDto(order));
    }
}
