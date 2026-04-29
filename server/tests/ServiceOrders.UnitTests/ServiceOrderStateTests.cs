using FluentAssertions;
using ServiceOrders.Domain.Exceptions;
using ServiceOrders.Domain.Entities.ServiceOrders;
using ServiceOrders.Domain.ValueObjects;
using Xunit;

namespace ServiceOrders.UnitTests;

public sealed class ServiceOrderStateTests
{
    private static readonly Guid UserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private static readonly Guid TechnicianId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

    [Fact]
    public void New_service_order_should_start_open()
    {
        var order = CreateOrder();

        order.Status.Should().Be(ServiceOrderStatus.Open);
        order.AssignedTechnicianId.Should().BeNull();
    }

    [Fact]
    public void Cannot_start_without_assigned_technician()
    {
        var order = CreateOrder();

        var action = () => order.Start(UserId);

        action.Should().Throw<DomainException>()
            .WithMessage("*atribuído*");
    }

    [Fact]
    public void Should_follow_assignment_and_execution_flow()
    {
        var order = CreateOrder();

        order.Assign(TechnicianId, UserId);
        order.Start(TechnicianId);
        order.Complete(TechnicianId, "Equipamento configurado e validado.");

        order.Status.Should().Be(ServiceOrderStatus.Completed);
        order.StartedAt.Should().NotBeNull();
        order.CompletedAt.Should().NotBeNull();
        order.History.Should().HaveCount(3);
    }

    [Fact]
    public void Cannot_cancel_completed_order()
    {
        var order = CreateOrder();
        order.Assign(TechnicianId, UserId);
        order.Start(TechnicianId);
        order.Complete(TechnicianId, "Finalizado.");

        var action = () => order.Cancel(UserId, "Erro operacional.");

        action.Should().Throw<DomainException>();
    }

    private static ServiceOrder CreateOrder() =>
        new(
            ServiceOrderNumber.Create("OS-TEST-00001"),
            "Instalação de roteador",
            "Cliente solicita instalação e configuração inicial.",
            "Cliente Exemplo",
            Email.Create("cliente@example.com"),
            ServiceOrderPriority.Medium,
            UserId,
            DateTimeOffset.UtcNow.AddDays(2));
}
