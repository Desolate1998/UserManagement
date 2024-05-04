using ErrorOr;
using MediatR;
using UserManagement.Backend.Common.JwtTokenGenerator;
using UserManagement.Backend.Common.PasswordHelper;
using UserManagement.Backend.Domain.Database;
using UserManagement.Backend.Domain.LookupCodes;
using UserManagement.Backend.Domain.Repositories_Interfaces;

namespace UserManagement.Backend.Application.Core.Authentication.Queries.LoginQuery;


public class LoginQueryHandler(IAuthenticationRepository repository, IJwtTokenGenerator tokenHelper) : IRequestHandler<LoginQuery, ErrorOr<LoginQueryResponseDTO>>
{
    async Task<ErrorOr<LoginQueryResponseDTO>> IRequestHandler<LoginQuery, ErrorOr<LoginQueryResponseDTO>>.Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await repository.GetUserByEmailAsync(request.Data.Email.ToLower());
        if (user is null)
        {
            await repository.LogLoginRequestAsync(UserLoginHistory.Create(null, LoginStatuses.FailedIncorrectEmail, request.AuthorizationDetails.Ip));
            return Error.Unauthorized("Unauthorized", "Invalid email, or password provided");
        }

        if (!PasswordHelper.ValidLoginPassword(request.Data.Password, user.PasswordSalt, user.PasswordHash))
        {
            await repository.LogLoginRequestAsync(UserLoginHistory.Create(user.EntryId, LoginStatuses.FailedIncorrectPassword, request.AuthorizationDetails.Ip));
            return Error.Unauthorized("Unauthorized", "Invalid email, or password provided");
        }

        await repository.LogLoginRequestAsync(UserLoginHistory.Create(user.EntryId, LoginStatuses.Success, request.AuthorizationDetails.Ip));

        return new LoginQueryResponseDTO()
        {
            JwtToken = tokenHelper.GenerateToken(user.Email, user.FirstName, user.LastName, user.EntryId)
        };
    }
}
