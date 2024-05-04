using ErrorOr;
using MediatR;
using UserManagement.Backend.Domain.Repositories_Interfaces;

namespace UserManagement.Backend.Application.Core.UserManagement.Commands.DeleteUser;

public class DeleteUserCommandHandler(IUserManagementRepository repository) : IRequestHandler<DeleteUserCommand, ErrorOr<bool>>
{
    async Task<ErrorOr<bool>> IRequestHandler<DeleteUserCommand, ErrorOr<bool>>.Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (!await repository.CheckIfAdminUserAsync(request.AuthorizationDetails.UserId))
        {
            return Error.Unauthorized("Unauthorized");
        }

        if(!await repository.CheckIfUserExistsAsync(request.Data.UserId))
        {
            return Error.NotFound("Email", "User was not found");
        }

        await repository.DeleteUserAsync(request.Data.UserId);

        return true;
    }
}

