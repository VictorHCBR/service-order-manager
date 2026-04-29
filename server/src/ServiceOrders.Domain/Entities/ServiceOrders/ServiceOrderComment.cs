using ServiceOrders.Domain.Common;
using ServiceOrders.Domain.Exceptions;

namespace ServiceOrders.Domain.Entities.ServiceOrders;

public class ServiceOrderComment : BaseEntity
{
    private ServiceOrderComment() { }

    public ServiceOrderComment(Guid serviceOrderId, Guid authorUserId, string message, bool isInternal)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new DomainException("Comentário não pode ser vazio.");

        ServiceOrderId = serviceOrderId;
        AuthorUserId = authorUserId;
        Message = message.Trim();
        IsInternal = isInternal;
    }

    public Guid ServiceOrderId { get; private set; }
    public Guid AuthorUserId { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public bool IsInternal { get; private set; }
}
