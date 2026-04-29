using ServiceOrders.Domain.Common;

namespace ServiceOrders.Domain.Entities.Users;

public static class RoleName
{
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string Technician = "Technician";
    public const string User = "User";
    
    public static readonly IReadOnlySet<string> Profiles = new HashSet<string> { 
        Admin, Manager, Technician, User 
    };
}
