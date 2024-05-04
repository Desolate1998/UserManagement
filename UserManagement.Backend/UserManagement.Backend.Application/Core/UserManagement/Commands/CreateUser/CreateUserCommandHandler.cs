using ErrorOr;
using MediatR;
using UserManagement.Backend.Common.PasswordHelper;
using UserManagement.Backend.Domain.Database;
using UserManagement.Backend.Domain.Repositories_Interfaces;

namespace UserManagement.Backend.Application.Core.UserManagement.Commands.CreateUser;

public class CreateUserCommandHandler(IUserManagementRepository repository) : IRequestHandler<CreateUserCommand, ErrorOr<User>>
{
    async Task<ErrorOr<User>> IRequestHandler<CreateUserCommand, ErrorOr<User>>.Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (!await repository.CheckIfAdminUserAsync(request.AuthorizationDetails.UserId))
        {
            return Error.Unauthorized("Unauthorized");
        }
        if (await repository.CheckIfUserExistsAsync(request.Data.Email.ToLower()))
        {
            return Error.Conflict("Email", "User Already exists");
        }
        var user = User.Create(request.Data.FirstName,
                               request.Data.LastName,
                               request.Data.Email.ToLower(),
                               PasswordHelper.GenerateStrongPassword());

        user = await repository.AddUserAsync(user);

        await repository.AssignUserGroupsAsync(request.Data.Groups.Select(x => UserGroup.Create(user.EntryId, x)).ToList());
        return new User() { 
            Email = user.Email ,
            FirstName = user.FirstName ,
            LastName = user.LastName ,
            EntryId = user.EntryId
        };
    }
}


