using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserManagement.Backend.Common.JwtTokenGenerator;
using UserManagement.Backend.Domain.Repositories_Interfaces;
using UserManagement.Backend.Infrastructure.DataContext;
using UserManagement.Backend.Infrastructure.DataContexts.Seeds;
using UserManagement.Backend.Infrastructure.Repositories;

namespace Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddJwtServices(configuration);

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder => 
            {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
            });
        });

        services.AddDbContext<ManagementContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Management"));
        });

        var context = services.BuildServiceProvider()
                              .GetRequiredService<ManagementContext>();

        Seed.SeedDataAndMigrate(context);

        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        services.AddScoped<IUserManagementRepository, UserManagementRepository>();

        return services;
    }
}
