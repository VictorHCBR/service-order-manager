namespace ServiceOrders.Domain.Entities.ServiceOrders;

public enum ServiceOrderStatus
{
    Open = 1,
    Assigned = 2,
    InProgress = 3,
    Paused = 4,
    WaitingCustomer = 5,
    Completed = 6,
    Cancelled = 7
}
