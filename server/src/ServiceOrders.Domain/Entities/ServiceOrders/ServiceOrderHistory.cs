using ServiceOrders.Domain.Common;

namespace ServiceOrders.Domain.Entities.ServiceOrders;

public sealed class ServiceOrderHistory : BaseEntity
{
    private ServiceOrderHistory() { }

    public ServiceOrderHistory(Guid serviceOrderId, ServiceOrderStatus from, ServiceOrderStatus to, Guid changeByUserId, string? reason)
    {
        ServiceOrderId = serviceOrderId;
        From = from;
        To = to;
        ChangedByUserId = changeByUserId;
        Reason = reason;
    }

    public Guid ServiceOrderId { get; private set; }
    public ServiceOrderStatus From { get; private set; }
    public ServiceOrderStatus To { get; private set; }
    public Guid ChangedByUserId { get; private set; }
    public string? Reason { get; private set; }
}
