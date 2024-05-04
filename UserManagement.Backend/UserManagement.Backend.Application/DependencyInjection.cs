using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Backend.Application.Common.Behaviors;
using UserManagement.Backend.Application.Core.Authentication.Queries.LoginQuery;


namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssemblyContaining<LoginQueryValidator>();
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection)));
        return services;
    }
}
