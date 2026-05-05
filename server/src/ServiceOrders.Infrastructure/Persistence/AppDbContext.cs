using Microsoft.EntityFrameworkCore;
using ServiceOrders.Domain.Entities.Users;
using ServiceOrders.Domain.Entities.ServiceOrders;
using ServiceOrders.Domain.ValueObjects;
using ServiceOrders.Infrastructure.Persistence.Converters;

namespace ServiceOrders.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<ServiceOrder> ServiceOrders => Set<ServiceOrder>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Email>()
            .HaveConversion<EmailValueConverter>()
            .HaveMaxLength(180);

        configurationBuilder.Properties<ServiceOrderNumber>()
            .HaveConversion<ServiceOrderNumberValueConverter>()
            .HaveMaxLength(30);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Email>();
        modelBuilder.Ignore<ServiceOrderNumber>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
