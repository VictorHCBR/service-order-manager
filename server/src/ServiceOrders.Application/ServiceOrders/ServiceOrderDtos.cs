using ServiceOrders.Domain.Entities.ServiceOrders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceOrders.Application.ServiceOrders;

public sealed record ServiceOrderSummaryDto(
    Guid Id,
    string Number,
    string Title,
    string CustomerName,
    string CustomerEmail,
    ServiceOrderPriority Priority,
    ServiceOrderStatus Status,
    Guid CreatedByUserId,
    Guid? AssignedTechnicianId,
    DateTimeOffset? DueDate,
    DateTimeOffset CreatedAt);

public sealed record ServiceOrderDetailsDto(
    Guid Id,
    string Number,
    string Title,
    string Description,
    string CustomerName,
    string CustomerEmail,
    ServiceOrderPriority Priority,
    ServiceOrderStatus Status,
    Guid CreatedByUserId,
    Guid? AssignedTechnicianId,
    DateTimeOffset? DueDate,
    DateTimeOffset? StartedAt,
    DateTimeOffset? CompletedAt,
    DateTimeOffset? CancelledAt,
    IReadOnlyList<ServiceOrderCommentDto> Comments,
    IReadOnlyList<ServiceOrderHistoryDto> History);

public sealed record ServiceOrderCommentDto(Guid Id, Guid AuthorUserId, string Message, bool IsInternal, DateTimeOffset CreatedAt);
public sealed record ServiceOrderHistoryDto(Guid Id, string From, string To, Guid ChangedByUserId, string? Reason, DateTimeOffset CreatedAt);
public sealed record CreateServiceOrderRequest(
    string Title,
    string Description,
    string CustomerName,
    string CustomerEmail,
    ServiceOrderPriority Priority,
    DateTimeOffset? DueDate);

public sealed record UpdateServiceOrderRequest(
    string Title,
    string Description,
    ServiceOrderPriority Priority,
    DateTimeOffset? DueDate);

public sealed record AssignServiceOrderRequest(Guid TechnicianId);
public sealed record CompleteServiceOrderRequest(string Resolution);
public sealed record CancelServiceOrderRequest(string Reason);
public sealed record PauseServiceOrderRequest(string Reason);
public sealed record AddCommentRequest(string Message, bool IsInternal);