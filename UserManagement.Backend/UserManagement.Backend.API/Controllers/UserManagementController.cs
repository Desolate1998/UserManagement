using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Backend.Application.Core.UserManagement.Commands.CreateUser;
using UserManagement.Backend.Application.Core.UserManagement.Commands.DeleteUser;
using UserManagement.Backend.Application.Core.UserManagement.Commands.EditUser;
using UserManagement.Backend.Application.Core.UserManagement.Queries.GetGroupsAndPermissions;
using UserManagement.Backend.Application.Core.UserManagement.Queries.GetStats;
using UserManagement.Backend.Application.Core.UserManagement.Queries.GetUsers;
using UserManagement.Backend.Common.ApplicationProviders;
using UserManagement.Backend.Common.AuthorizationDetails;

namespace UserManagement.Backend.API.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class UserManagementController(ILogger<UserManagementController> logger, ISender mediator, IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    [HttpPost("Create")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDTO request)
    {
        logger.LogInformation($"Create Users request received at [{DateTimeProvider.ApplicationDate}]");
        CreateUserCommand query = new(request, AuthorizationDetails.Create(httpContextAccessor));
        return Ok(await mediator.Send(query));
    }

    [HttpGet("Users")]
    public async Task<IActionResult> GetUsers()
    {
        logger.LogInformation($"Get Users request received at [{DateTimeProvider.ApplicationDate}]");
        GetUsersQuery query = new(AuthorizationDetails.Create(httpContextAccessor));
        return Ok(await mediator.Send(query));
    }

    [HttpPost("Edit")]
    public async Task<IActionResult> EditUser([FromBody] EditUserRequestDTO request)
    {
        logger.LogInformation($"Edit Users request received at [{DateTimeProvider.ApplicationDate}]");
        EditUserCommand query = new(request, AuthorizationDetails.Create(httpContextAccessor));
        return Ok(await mediator.Send(query));
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> DeleteUser([FromBody] DeleteUserCommandRequestDTO request)
    {
        logger.LogInformation($"Delete Users request received at [{DateTimeProvider.ApplicationDate}]");
        DeleteUserCommand query = new(request, AuthorizationDetails.Create(httpContextAccessor));
        return Ok(await mediator.Send(query));
    }

    [HttpGet("GetGroupsAndPermissions")]
    public async Task<IActionResult> GetGroupsAndPermissions()
    {
        logger.LogInformation($"GetGroupsAndPermissions request received at [{DateTimeProvider.ApplicationDate}]");
        GetGroupsAndPermissionsQuery query = new(AuthorizationDetails.Create(httpContextAccessor));
        return Ok(await mediator.Send(query));
    }

    [HttpGet("GetStats")]
    public async Task<IActionResult> GetStats()
    {
        logger.LogInformation($"GetStatsQuery request received at [{DateTimeProvider.ApplicationDate}]");
        GetStatsQuery query = new(AuthorizationDetails.Create(httpContextAccessor));
        return Ok(await mediator.Send(query));
    }
}
