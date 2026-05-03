using Microsoft.Extensions.DependencyInjection;
using ServiceOrders.Application.ServiceOrders.Commands;

namespace ServiceOrders.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateServiceOrderCommandHandler>();
        services.AddScoped<UpdateServiceOrderCommandHandler>();
        services.AddScoped<AssignServiceOrderCommandHandler>();
        services.AddScoped<StartServiceOrderCommandHandler>();
        services.AddScoped<PauseServiceOrderCommandHandler>();
        services.AddScoped<CompleteServiceOrderCommandHandler>();
        services.AddScoped<CancelServiceOrderCommandHandler>();
        services.AddScoped<AddServiceOrderCommentCommandHandler>();

        return services;
    }
}
