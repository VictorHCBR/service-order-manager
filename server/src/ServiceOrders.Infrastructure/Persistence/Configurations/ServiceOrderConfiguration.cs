using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceOrders.Domain.Entities.ServiceOrders;
using ServiceOrders.Domain.ValueObjects;

namespace ServiceOrders.Infrastructure.Persistence.Configurations;

public sealed class ServiceOrderConfiguration : IEntityTypeConfiguration<ServiceOrder>
{
    public void Configure(EntityTypeBuilder<ServiceOrder> builder)
    {
        builder.ToTable("service_orders");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Number)
            .HasConversion(
                number => number.Value,
                value => ServiceOrderNumber.Create(value))
            .HasColumnName("number")
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(x => x.Number).IsUnique();

        builder.Property(x => x.Title).HasColumnName("title").HasMaxLength(160).IsRequired();
        builder.Property(x => x.Description).HasColumnName("description").HasMaxLength(5000).IsRequired();
        builder.Property(x => x.CustomerName).HasColumnName("customer_name").HasMaxLength(160).IsRequired();

        builder.Property(x => x.CustomerEmail)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value))
            .HasColumnName("customer_email")
            .HasMaxLength(180)
            .IsRequired();

        builder.Property(x => x.Priority).HasColumnName("priority").HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.CreatedByUserId).HasColumnName("created_by_user_id");
        builder.Property(x => x.AssignedTechnicianId).HasColumnName("assigned_technician_id");
        builder.Property(x => x.DueDate).HasColumnName("due_date");
        builder.Property(x => x.StartedAt).HasColumnName("started_at");
        builder.Property(x => x.CompletedAt).HasColumnName("completed_at");
        builder.Property(x => x.CancelledAt).HasColumnName("cancelled_at");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.AssignedTechnicianId);
        builder.HasIndex(x => x.DueDate);

        builder.Navigation(x => x.Comments)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .AutoInclude();
        builder.OwnsMany(x => x.Comments, comments =>
        {
            comments.ToTable("service_order_comments");
            comments.WithOwner().HasForeignKey("service_order_id");
            comments.HasKey("Id");
            comments.Property(c => c.Id).HasColumnName("id");
            comments.Property(c => c.AuthorUserId).HasColumnName("author_user_id");
            comments.Property(c => c.Message).HasColumnName("message").HasMaxLength(4000).IsRequired();
            comments.Property(c => c.IsInternal).HasColumnName("is_internal");
            comments.Property(c => c.CreatedAt).HasColumnName("created_at");
            comments.Property(c => c.UpdatedAt).HasColumnName("updated_at");
        });

        builder.Navigation(x => x.History)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .AutoInclude();
        builder.OwnsMany(x => x.History, history =>
        {
            history.ToTable("service_order_history");
            history.WithOwner().HasForeignKey("service_order_id");
            history.HasKey("Id");
            history.Property(h => h.Id).HasColumnName("id");
            history.Property(h => h.From).HasColumnName("from_status").HasConversion<string>().HasMaxLength(30);
            history.Property(h => h.To).HasColumnName("to_status").HasConversion<string>().HasMaxLength(30);
            history.Property(h => h.ChangedByUserId).HasColumnName("changed_by_user_id");
            history.Property(h => h.Reason).HasColumnName("reason").HasMaxLength(1000);
            history.Property(h => h.CreatedAt).HasColumnName("created_at");
            history.Property(h => h.UpdatedAt).HasColumnName("updated_at");
        });
    }
}
