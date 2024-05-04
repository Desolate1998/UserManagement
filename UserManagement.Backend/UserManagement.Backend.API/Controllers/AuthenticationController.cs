using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Backend.Application.Core.Authentication.Queries.LoginQuery;
using UserManagement.Backend.Common.ApplicationProviders;
using UserManagement.Backend.Common.AuthorizationDetails;

namespace UserManagement.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(ILogger<AuthenticationController> logger, ISender mediator, IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        /// <summary>
        /// Handles the login query.
        /// </summary>
        /// <param name="request">The query containing the login details.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>
        ///   An <see cref="ErrorOr{LoginQueryResponse}"/> indicating the result of the operation. If successful, returns a <see cref="LoginQueryResponseDTO"/> containing user information and authentication tokens.
        ///   Otherwise, returns an <see cref="Error"/> indicating the reason for failure.
        ///   <list type="bullet">
        ///     <item><see cref="Error.Unauthorized(string, string)"/></item> 
        ///   </list>
        /// </returns>
        [HttpPost("Login")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ErrorOr<LoginQueryResponseDTO>), 200)]
        public async Task<IActionResult> Login([FromBody]LoginQueryRequestDTO request)
        {
            logger.LogInformation($"Login request received at [{DateTimeProvider.ApplicationDate}]");
            LoginQuery query = new(request, AuthorizationDetails.Create(httpContextAccessor));
            return Ok(await mediator.Send(query));
        }
    }
}
