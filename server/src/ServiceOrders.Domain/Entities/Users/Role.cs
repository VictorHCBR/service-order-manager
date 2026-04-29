using ServiceOrders.Domain.Common;
using ServiceOrders.Domain.Exceptions;

namespace ServiceOrders.Domain.Entities.Users;

public sealed class Role : BaseEntity
{
    private Role() { }

    public Role(string name)
    {
        if (!RoleName.Profiles.Contains(name))
            throw new DomainException($"Perfil \"{name}\" não é suportado.");
    }

    public Guid UserId { get; private set; }
    public string Name { get; private set; } = string.Empty;

    public void AttachToUser(Guid userId) => UserId = userId;
}
