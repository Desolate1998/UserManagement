using ErrorOr;
using MediatR;
using UserManagement.Backend.Domain.Database;
using UserManagement.Backend.Domain.LookupCodes;
using UserManagement.Backend.Domain.Repositories_Interfaces;

namespace UserManagement.Backend.Application.Core.UserManagement.Commands.EditUser;

public class EditUserCommandHandler(IUserManagementRepository repository) : IRequestHandler<EditUserCommand, ErrorOr<bool>>
{
    async Task<ErrorOr<bool>> IRequestHandler<EditUserCommand, ErrorOr<bool>>.Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        if (!await repository.CheckIfAdminUserAsync(request.AuthorizationDetails.UserId))
        {
            return Error.Unauthorized("Unauthorized");
        }

        var user = await repository.GetUserByIdAsync(request.Data.EntryId);

        if (user is null)
        {
            return Error.NotFound("NotFound", "User Does Not Exist");
        }

        if (request.Data.Email.Changed && await repository.CheckIfUserExistsAsync(request.Data.Email.Value!))
        {
            return Error.Conflict("Email", "Email Already exists");
        }

        user.Email = request.Data.Email.Value!;
        user.FirstName = request.Data.FirstName;
        user.FirstName = request.Data.LastName;

        try
        {
            await repository.BeginTransactionAsync();

            await repository.UpdateUserAsync(user);

            var groupsToRemove = new List<string>();
            var groupsToAdd = new List<UserGroup>();

            if (request.Data.UserGroups.Where(x => x.Changed != UserGroupChangeCodes.None).Any())
            {
                groupsToAdd = request.Data.UserGroups.Where(x => x.Changed == UserGroupChangeCodes.Added).Select(x => UserGroup.Create(user.EntryId, x.Value!)).ToList();
                groupsToRemove = request.Data.UserGroups.Where(x => x.Changed == UserGroupChangeCodes.Removed).Select(x => x.Value!).ToList();
                await repository.UpdateUserGroupsAsync(groupsToRemove, groupsToAdd, user.EntryId);
                user = await repository.GetUserByIdAsync(request.Data.EntryId);
            }
            await repository.CommitTransactionAsync();
        }
        catch 
        {
            await repository.RollbackTransactionAsync();
            throw;
        }
       return true;
    }
}