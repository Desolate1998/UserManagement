using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Backend.Application.Core.Authentication.Queries.LoginQuery;
using UserManagement.Backend.Common.AuthorizationDetails;
using UserManagement.Backend.Common.JwtTokenGenerator;
using UserManagement.Backend.Domain.Database;
using UserManagement.Backend.Domain.Repositories_Interfaces;

namespace UserManagement.Backend.Tests.ApplicationTests;

[TestClass]
public class LoginQueryHandlerTests
{
    [TestMethod]
    public async Task Handle_Valid_Login()
    {
        var jwtSettings = new JwtSettings
        {
            Secret = "PewPewPewPewPewPewPewPewPewPewPewPewPewPew",
            ExpiryMinutes = 120,
            Issuer = "PewPew",
            Audience = "PewPew"
        };
        var services = new ServiceCollection();
        var mockJwtTokenGen = new Mock<JwtTokenGenerator>();
        var mockHttpContext = new Mock<HttpContextAccessor>();
        var optionsMock = new Mock<IOptions<JwtSettings>>();
        optionsMock.Setup(x => x.Value).Returns(jwtSettings);
        var jwtTokenGenerator = new JwtTokenGenerator(optionsMock.Object);
        var mockRepository = new Mock<IAuthenticationRepository>();
        var user = User.Create("Meow", "meower", "meow@meow", "meowww");
        mockRepository.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>()))
                      .ReturnsAsync(user);

        mockRepository.Setup(repo => repo.LogLoginRequestAsync(It.IsAny<UserLoginHistory>()))
            .Returns(Task.CompletedTask);

        var serviceProvider = services.AddSingleton<IJwtTokenGenerator>(jwtTokenGenerator)
                                      .AddScoped(_ => mockRepository.Object)
                                      .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(LoginQueryHandler).Assembly))
                                      .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var query = new LoginQuery(new() { Email = "meow@meow", Password = "meowww" },
                              AuthorizationDetails.Create(mockHttpContext.Object));

        var results = await mediator.Send(query);
        Assert.IsFalse(results.IsError);
        Assert.IsNotNull(results.Value);
    }

    [TestMethod]
    public async Task Handle_Invalid_User_Password()
    {
        var jwtSettings = new JwtSettings
         {
             Secret = "PewPewPewPewPewPewPewPewPewPewPewPewPewPew",
             ExpiryMinutes = 120,
             Issuer = "PewPew",
             Audience = "PewPew"
         };
        var services = new ServiceCollection();
        var mockJwtTokenGen = new Mock<JwtTokenGenerator>();
        var mockHttpContext = new Mock<HttpContextAccessor>();
        var optionsMock = new Mock<IOptions<JwtSettings>>();
        optionsMock.Setup(x => x.Value).Returns(jwtSettings);
        var jwtTokenGenerator = new JwtTokenGenerator(optionsMock.Object);
        var mockRepository = new Mock<IAuthenticationRepository>();

        mockRepository.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>()))
                      .ReturnsAsync(User.Create("Meow", "meower", "meow@meow","meowww"));

        mockRepository.Setup(repo => repo.LogLoginRequestAsync(It.IsAny<UserLoginHistory>()))
            .Returns(Task.CompletedTask);

        var serviceProvider = services.AddSingleton<IJwtTokenGenerator>(jwtTokenGenerator)
                                      .AddScoped(_ => mockRepository.Object)
                                      .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(LoginQueryHandler).Assembly))
                                      .BuildServiceProvider();
        
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var query = new LoginQuery(new() { Email = "", Password = "" },
                              AuthorizationDetails.Create(mockHttpContext.Object));

        var results = await mediator.Send(query);
        Assert.IsTrue(results.IsError);
        Assert.AreEqual(results.Errors[0].Code, "Unauthorized");
        Assert.AreEqual(results.Errors[0].Description, "Invalid email, or password provided");
    }

    [TestMethod]
    public async Task Handle_Invalid_User_Email()
    {
        var jwtSettings = new JwtSettings
        {
            Secret = "PewPewPewPewPewPewPewPewPewPewPewPewPewPew",
            ExpiryMinutes = 120,
            Issuer = "PewPew",
            Audience = "PewPew"
        };
        var services = new ServiceCollection();
        var mockJwtTokenGen = new Mock<JwtTokenGenerator>();
        var mockHttpContext = new Mock<HttpContextAccessor>();
        var optionsMock = new Mock<IOptions<JwtSettings>>();
        optionsMock.Setup(x => x.Value).Returns(jwtSettings);
        var jwtTokenGenerator = new JwtTokenGenerator(optionsMock.Object);
        var mockRepository = new Mock<IAuthenticationRepository>();
        var serviceProvider = services.AddSingleton<IJwtTokenGenerator>(jwtTokenGenerator)
                .AddScoped(_ => mockRepository.Object)
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(LoginQueryHandler).Assembly))
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var query = new LoginQuery(new() { Email = "", Password = "" },
                              AuthorizationDetails.Create(mockHttpContext.Object));

        var results = await mediator.Send(query);
        Assert.IsTrue(results.IsError);
        Assert.AreEqual(results.Errors[0].Code, "Unauthorized");
        Assert.AreEqual(results.Errors[0].Description, "Invalid email, or password provided");
    }
}