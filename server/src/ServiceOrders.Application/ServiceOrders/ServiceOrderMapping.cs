using ServiceOrders.Domain.Entities.ServiceOrders;

namespace ServiceOrders.Application.ServiceOrders;

internal static class ServiceOrderMapping
{
    public static ServiceOrderSummaryDto ToSummaryDto(this ServiceOrder order) =>
        new(
            order.Id,
            order.Number.Value,
            order.Title,
            order.CustomerName,
            order.CustomerEmail.Value,
            order.Priority,
            order.Status,
            order.CreatedByUserId,
            order.AssignedTechnicianId,
            order.DueDate,
            order.CreatedAt);

    public static ServiceOrderDetailsDto ToDetailsDto(this ServiceOrder order) =>
        new(
            order.Id,
            order.Number.Value,
            order.Title,
            order.Description,
            order.CustomerName,
            order.CustomerEmail.Value,
            order.Priority,
            order.Status,
            order.CreatedByUserId,
            order.AssignedTechnicianId,
            order.DueDate,
            order.StartedAt,
            order.CompletedAt,
            order.CancelledAt,
            [.. order.Comments.Select(x => new ServiceOrderCommentDto(x.Id, x.AuthorUserId, x.Message, x.IsInternal, x.CreatedAt))],
            [.. order.History.Select(x => new ServiceOrderHistoryDto(x.Id, x.From.ToString(), x.To.ToString(), x.ChangedByUserId, x.Reason, x.CreatedAt))]);

}
