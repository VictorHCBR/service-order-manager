using Microsoft.Extensions.DependencyInjection;
using ServiceOrders.Application.Auth;
using ServiceOrders.Application.ServiceOrders.Commands;
using ServiceOrders.Application.ServiceOrders.Queries;

namespace ServiceOrders.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<CreateServiceOrderCommandHandler>();
        services.AddScoped<UpdateServiceOrderCommandHandler>();
        services.AddScoped<AssignServiceOrderCommandHandler>();
        services.AddScoped<StartServiceOrderCommandHandler>();
        services.AddScoped<PauseServiceOrderCommandHandler>();
        services.AddScoped<CompleteServiceOrderCommandHandler>();
        services.AddScoped<CancelServiceOrderCommandHandler>();
        services.AddScoped<AddServiceOrderCommentCommandHandler>();
        services.AddScoped<ListServiceOrdersQueryHandler>();
        services.AddScoped<GetServiceOrderByIdQueryHandler>();

        return services;
    }
}
