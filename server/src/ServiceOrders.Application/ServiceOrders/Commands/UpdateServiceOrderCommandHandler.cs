using ServiceOrders.Application.Abstractions;
using ServiceOrders.Domain.Common;

namespace ServiceOrders.Application.ServiceOrders.Commands;

public sealed class UpdateServiceOrderCommandHandler(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<ServiceOrderDetailsDto>> HandleAsync(Guid id, UpdateServiceOrderRequest request, CancellationToken token)
    {
        var order = await _unitOfWork.ServiceOrders.GetByIdAsync(id);
        if (order is null)
            return Result<ServiceOrderDetailsDto>.Failure("OS não encontrada!");

        order.UpdateDetails(request.Title, request.Description, request.Priority, request.DueDate);
        _unitOfWork.ServiceOrders.Update(order);
        await _unitOfWork.SaveChangesAsync(token);

        return Result<ServiceOrderDetailsDto>.Success(order.ToDetailsDto());
    }
}
