using ServiceOrders.Domain.Common;
using ServiceOrders.Domain.Exceptions;
using ServiceOrders.Domain.ValueObjects;

namespace ServiceOrders.Domain.Entities.Users;

public sealed class User : BaseEntity, IAggregateRoot
{
    private User() { }

    public User(string name, Email email, string passwordHash, IEnumerable<Role> roles)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Nome do usuário é obrigatório!");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Hash da senha é obrigatório");

        Name = name.Trim();
        Email = email;
        PasswordHash = passwordHash;

        foreach (var role in roles.DistinctBy(x => x.Name))
            AddRole(role);

        if (Roles.Count == 0)
            AddRole(new Role(RoleName.User));
    }
    
    public string Name { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;

    public List<Role> Roles { get; private set; } = [];

    public void Deactivate(){
        if (!IsActive) return;
        IsActive = false;
        Touch();
    }

    public void Activate()
    {
        if(IsActive) return;
        IsActive = true;
        Touch();
    }
    
    public void AddRole(Role role)
    {
        if(Roles.Any(x => x.Name == role.Name))
            return;

        role.AttachToUser(Id);
        Roles.Add(role);Touch();
    }

    public bool HasRole(string roleName) =>
        Roles.Any(x => x.Name == roleName);
}