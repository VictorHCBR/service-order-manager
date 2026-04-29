using ServiceOrders.Domain.Entities.ServiceOrders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceOrders.Application.ServiceOrders;

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