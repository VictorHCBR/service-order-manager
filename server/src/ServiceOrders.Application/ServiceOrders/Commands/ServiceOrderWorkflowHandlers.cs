using ServiceOrders.Application.Abstractions;
using ServiceOrders.Domain.Common;

namespace ServiceOrders.Application.ServiceOrders.Commands;

public sealed class AssignServiceOrderCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<ServiceOrderDetailsDto>> HandleAsync(Guid id, AssignServiceOrderRequest request, CancellationToken token)
    {
        var order = await _unitOfWork.ServiceOrders.GetByIdAsync(id);
        if (order is null)
            return Result<ServiceOrderDetailsDto>.Failure("OS não encontrada");

        order.Assign(request.TechnicianId, _currentUser.UserId);
        await _unitOfWork.SaveChangesAsync(token);

        return Result<ServiceOrderDetailsDto>.Success(order.ToDetailsDto());
    }
}

public sealed class StartServiceOrderCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<ServiceOrderDetailsDto>> HandleAsync(Guid id, CancellationToken token)
    {
        var order = await _unitOfWork.ServiceOrders.GetByIdAsync(id, token);
        if (order is null)
            return Result<ServiceOrderDetailsDto>.Failure("OS não encontrada.");

        order.Start(_currentUser.UserId);
        await _unitOfWork.SaveChangesAsync(token);

        return Result<ServiceOrderDetailsDto>.Success(order.ToDetailsDto());
    }
}

public sealed class CompleteServiceOrderCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<ServiceOrderDetailsDto>> HandleAsync(Guid id, CompleteServiceOrderRequest request, CancellationToken token)
    {
        var order = await _unitOfWork.ServiceOrders.GetByIdAsync(id, token);
        if (order is null)
            return Result<ServiceOrderDetailsDto>.Failure("OS não encontrada.");

        order.Complete(_currentUser.UserId, request.Resolution);
        await _unitOfWork.SaveChangesAsync(token);

        return Result<ServiceOrderDetailsDto>.Success(order.ToDetailsDto());
    }
}

public sealed class CancelServiceOrderCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<ServiceOrderDetailsDto>> HandleAsync(Guid id, CancelServiceOrderRequest request, CancellationToken token)
    {
        var order = await _unitOfWork.ServiceOrders.GetByIdAsync(id, token);
        if (order is null)
            return Result<ServiceOrderDetailsDto>.Failure("OS não encontrada");

        order.Cancel(_currentUser.UserId, request.Reason);
        await _unitOfWork.SaveChangesAsync(token);

        return Result<ServiceOrderDetailsDto>.Success(order.ToDetailsDto());
    }
}

public sealed class PauseServiceOrderCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<ServiceOrderDetailsDto>> HandleAsync(Guid id, PauseServiceOrderRequest request, CancellationToken token)
    {
        var order = await _unitOfWork.ServiceOrders.GetByIdAsync(_currentUser.UserId, token);
        if (order is null)
            return Result<ServiceOrderDetailsDto>.Failure("OS não encontrada");

        order.Pause(_currentUser.UserId, request.Reason);
        await _unitOfWork.SaveChangesAsync(token);

        return Result<ServiceOrderDetailsDto>.Success(order.ToDetailsDto());
    }
}

public sealed class AddServiceOrderCommentCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<ServiceOrderDetailsDto>> HandleAsync(Guid id, AddCommentRequest request, CancellationToken token)
    {
        var order = await _unitOfWork.ServiceOrders.GetByIdAsync(id, token);
        if (order is null)
            return Result<ServiceOrderDetailsDto>.Failure("OS não encontrada");

        order.AddComment(_currentUser.UserId, request.Message, request.IsInternal);
        await _unitOfWork.SaveChangesAsync(token);

        return Result<ServiceOrderDetailsDto>.Success(order.ToDetailsDto());
    }
}

