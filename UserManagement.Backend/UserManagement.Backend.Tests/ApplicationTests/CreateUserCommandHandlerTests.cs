using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Backend.Application.Core.Authentication.Queries.LoginQuery;
using UserManagement.Backend.Application.Core.UserManagement.Commands.CreateUser;
using UserManagement.Backend.Common.AuthorizationDetails;
using UserManagement.Backend.Common.JwtTokenGenerator;
using UserManagement.Backend.Domain.Database;
using UserManagement.Backend.Domain.Repositories_Interfaces;

namespace UserManagement.Backend.Tests.ApplicationTests;

[TestClass]
public class CreateUserCommandHandlerTests
{
    [TestMethod]
    public async Task Handle_Valid_Create()
    {
        var jwtSettings = new JwtSettings
        {
            Secret = "PewPewPewPewPewPewPewPewPewPewPewPewPewPew",
            ExpiryMinutes = 120,
            Issuer = "PewPew",
            Audience = "PewPew"
        };
        var CreateUserRequestDTO = new CreateUserRequestDTO() {
            Email = "meow@meow",
            FirstName = "meow",
            Password = "meow",
            LastName = "meow",
            Groups = ["GRP1"]
        };

        var services = new ServiceCollection();
        var mockHttpContext = new Mock<HttpContextAccessor>();
     
     
        var mockRepository = new Mock<IUserManagementRepository>();
        var user = User.Create("Meow", "meower", "meow@meow", "meowww");
        user.EntryId = 1;
        mockRepository.Setup(repo => repo.CheckIfAdminUserAsync(It.IsAny<long>()))
                      .ReturnsAsync(true);

        mockRepository.Setup(repo => repo.CheckIfUserExistsAsync(It.IsAny<string>()))
                      .ReturnsAsync(false);

        mockRepository.Setup(repo => repo.AddUserAsync(It.IsAny<User>()))
                      .ReturnsAsync(user);
        var serviceProvider = services.AddScoped(_ => mockRepository.Object)
                                      .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(LoginQueryHandler).Assembly))
                                      .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var query = new CreateUserCommand(CreateUserRequestDTO, AuthorizationDetails.Create(mockHttpContext.Object)); 

        var results = await mediator.Send(query);
        Assert.IsFalse(results.IsError);
        Assert.IsNotNull(results.Value);
    }

    [TestMethod]
    public async Task Handle_User_Already_Exists()
    {
        var jwtSettings = new JwtSettings
        {
            Secret = "PewPewPewPewPewPewPewPewPewPewPewPewPewPew",
            ExpiryMinutes = 120,
            Issuer = "PewPew",
            Audience = "PewPew"
        };
        var CreateUserRequestDTO = new CreateUserRequestDTO()
        {
            Email = "meow@meow",
            FirstName = "meow",
            Password = "meow",
            LastName = "meow",
            Groups = ["GRP1"]
        };

        var services = new ServiceCollection();
        var mockHttpContext = new Mock<HttpContextAccessor>();

        var mockRepository = new Mock<IUserManagementRepository>();
        var user = User.Create("Meow", "meower", "meow@meow", "meowww");
        user.EntryId = 1;

        mockRepository.Setup(repo => repo.CheckIfAdminUserAsync(It.IsAny<long>()))
                      .ReturnsAsync(true);

        mockRepository.Setup(repo => repo.CheckIfUserExistsAsync(It.IsAny<string>()))
                      .ReturnsAsync(true);

        mockRepository.Setup(repo => repo.AddUserAsync(It.IsAny<User>()))
                      .ReturnsAsync(user);

        var serviceProvider = services.AddScoped(_ => mockRepository.Object)
                                      .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(LoginQueryHandler).Assembly))
                                      .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var query = new CreateUserCommand(CreateUserRequestDTO, AuthorizationDetails.Create(mockHttpContext.Object));

        var results = await mediator.Send(query);
        Assert.IsTrue(results.IsError);
        Assert.AreEqual(results.Errors[0].Code, "Email");
        Assert.AreEqual(results.Errors[0].Description, "User Already exists");
    }

    public async Task Handle_User_Not_Admin()
    {
        var jwtSettings = new JwtSettings
        {
            Secret = "PewPewPewPewPewPewPewPewPewPewPewPewPewPew",
            ExpiryMinutes = 120,
            Issuer = "PewPew",
            Audience = "PewPew"
        };
        var CreateUserRequestDTO = new CreateUserRequestDTO()
        {
            Email = "meow@meow",
            FirstName = "meow",
            Password = "meow",
            LastName = "meow",
            Groups = ["GRP1"]
        };

        var services = new ServiceCollection();
        var mockHttpContext = new Mock<HttpContextAccessor>();

        var mockRepository = new Mock<IUserManagementRepository>();
        var user = User.Create("Meow", "meower", "meow@meow", "meowww");
        user.EntryId = 1;

        mockRepository.Setup(repo => repo.CheckIfAdminUserAsync(It.IsAny<long>()))
                      .ReturnsAsync(false);

        mockRepository.Setup(repo => repo.AddUserAsync(It.IsAny<User>()))
                      .ReturnsAsync(user);

        var serviceProvider = services.AddScoped(_ => mockRepository.Object)
                                      .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(LoginQueryHandler).Assembly))
                                      .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var query = new CreateUserCommand(CreateUserRequestDTO, AuthorizationDetails.Create(mockHttpContext.Object));

        var results = await mediator.Send(query);
        Assert.IsTrue(results.IsError);
        Assert.AreEqual(results.Errors[0].Code, "Unauthorized");
    }
}
