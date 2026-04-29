using ServiceOrders.Domain.Common;
using ServiceOrders.Domain.Exceptions;
using ServiceOrders.Domain.ValueObjects;

namespace ServiceOrders.Domain.Entities.ServiceOrders;

public  class ServiceOrder : BaseEntity, IAggregateRoot
{
    private readonly List<ServiceOrderHistory> _history = [];
    private readonly List<ServiceOrderComment> _comments = [];

    private ServiceOrder() { }

    public ServiceOrder(
        ServiceOrderNumber number,
        string title,
        string description,
        string customerName,
        Email customerEmail,
        ServiceOrderPriority priority,
        Guid createdByUserId,
        DateTimeOffset? dueDate
    )
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Título da OS é obrigatório.");

        if(title.Length > 160)
            throw new DomainException("Título da OS não deve exceder 160 caracteres.");

        if(string.IsNullOrWhiteSpace(description))
            throw new DomainException("Descrição da OS é obrigatória.");

        if(string.IsNullOrEmpty(customerName))
            throw new DomainException("O nome do cliente é obrigatório.");

        if(dueDate is not null && dueDate.Value <= DateTimeOffset.UtcNow)
            throw new DomainException("Prazo deve ser uma data futura.");

        Number = number;
        Title = title;
        Description = description;
        CustomerName = customerName;
        CustomerEmail = customerEmail;
        Priority = priority;
        CreatedByUserId = createdByUserId;
        DueDate = dueDate;
        Status = ServiceOrderStatus.Open;
    }

    public ServiceOrderNumber Number { get; private set; } = null!;
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string CustomerName { get; private set; } = string.Empty;
    public Email CustomerEmail { get; private set; } = null!;
    public ServiceOrderPriority Priority { get; private set; }
    public ServiceOrderStatus Status { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public Guid? AssignedTechnicianId { get; private set; }
    public DateTimeOffset? DueDate { get; private set; }
    public DateTimeOffset? StartedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public DateTimeOffset? CancelledAt { get; private set; }

    public IReadOnlyCollection<ServiceOrderHistory> History => _history.AsReadOnly();
    public IReadOnlyCollection<ServiceOrderComment> Comments => _comments.AsReadOnly();

    public void UpdateDetails(string title, string description, ServiceOrderPriority priority, DateTimeOffset? dueDate)
    {
        EnsureCanEdit();
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("O Título é obrigatório!");

        if(string.IsNullOrWhiteSpace(description))
            throw new DomainException("A Descrição é obrigatória!");

        if (dueDate is not null && dueDate.Value <= DateTimeOffset.UtcNow)
            throw new DomainException("Prazo deve ser uma data futura.");

        Title = title.Trim();
        Description = description.Trim();
        Priority = priority;
        DueDate = dueDate;
        Touch();
    }

    public void Assign(Guid technicianId, Guid changedByUserId)
    {
        EnsureCanEdit();
        if (technicianId == Guid.Empty)
            throw new DomainException("É obrigatório atribuir a um técnico!");

        AssignedTechnicianId = technicianId;
        ChangeStatus(ServiceOrderStatus.Assigned, changedByUserId, "Atribuição do Técnico");
    }

    public void Start(Guid changedByUserId)
    {
        if(AssignedTechnicianId is null)
            throw new DomainException("Não é possível iniciar a OS sem um técnico atribuído.");

        if (Status is not ServiceOrderStatus.Assigned and not ServiceOrderStatus.Paused and not ServiceOrderStatus.WaitingCustomer)
            throw new DomainException($"Não é possível iniciar a OS no status {Status}.");

        StartedAt ??= DateTimeOffset.UtcNow;
        ChangeStatus(ServiceOrderStatus.InProgress, changedByUserId, "Início/retomada da execução.");
    }

    public void Pause(Guid changedByUserId, string reason)
    {
        if (Status is not ServiceOrderStatus.InProgress)
            throw new DomainException($"Não é possível pausar a OS no status {Status}.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException("Motivo da Pausa é obrigatório.");

        ChangeStatus(ServiceOrderStatus.Paused, changedByUserId, reason);
    }

    public void WaitCustomer(Guid changedByUserId, string reason)
    {
        if (Status is not ServiceOrderStatus.InProgress)
            throw new DomainException("Apenas OS em andamento pode aguardar cliente.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException("Motivo é obrigatório.");

        ChangeStatus(ServiceOrderStatus.WaitingCustomer, changedByUserId, reason);
    }

    public void Complete(Guid changedByUserId, string resolution)
    {
        if (Status is not ServiceOrderStatus.InProgress)
            throw new DomainException("Apenas OS em andamento pode ser finalizada!");

        if (string.IsNullOrWhiteSpace(resolution))
            throw new DomainException("Resolução técnica é obrigatória para finalizar.");

        CompletedAt = DateTimeOffset.UtcNow;
        _comments.Add(new ServiceOrderComment(Id, changedByUserId, $"Resolução: {resolution.Trim()}", true));
        ChangeStatus(ServiceOrderStatus.Completed, changedByUserId, "Finalização com resolução técnica.");
    }

    public void Cancel(Guid changedByUserId, string reason) { 
        if (Status is ServiceOrderStatus.Completed or ServiceOrderStatus.Cancelled)
            throw new DomainException("OS finalizada ou já cancelada não pode ser cancelada!");

        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException("Motivo de Cancelamento é obrigatório.");

        CancelledAt = DateTimeOffset.UtcNow;
        ChangeStatus(ServiceOrderStatus.Cancelled, changedByUserId, reason);
    }

    public void AddComment(Guid authorUserId, string message, bool isInternal)
    {
        if (Status is ServiceOrderStatus.Cancelled)
            throw new DomainException("Não é permitido comentar uma OS cancelada.");

        _comments.Add(new ServiceOrderComment(Id, authorUserId, message, isInternal));
        Touch();
    }

    private void EnsureCanEdit()
    {
        if(Status is ServiceOrderStatus.Completed or ServiceOrderStatus.Cancelled)
            throw new DomainException("OS finalizada ou cancelada não pode ser editada"); 
    }

    private void ChangeStatus(ServiceOrderStatus next, Guid changedByUserId, string? reason)
    {
        if (Status == next)
            return;

        var old = Status;
        Status = next;
        _history.Add(new ServiceOrderHistory(Id, old, next, changedByUserId, reason));
        Touch();
    }
}
