using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceOrders.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.Now;
    public DateTimeOffset? UpdatedAt { get; protected set; }

    public void Touch() => UpdatedAt = DateTimeOffset.UtcNow;
}
