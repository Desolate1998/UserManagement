using ErrorOr;
using MediatR;
using UserManagement.Backend.Domain.Database;
using UserManagement.Backend.Domain.Repositories_Interfaces;

namespace UserManagement.Backend.Application.Core.UserManagement.Queries.GetUsers;

public class GetUsersQueryHandler(IUserManagementRepository repository) : IRequestHandler<GetUsersQuery, ErrorOr<List<User>>>
{
    async Task<ErrorOr<List<User>>> IRequestHandler<GetUsersQuery, ErrorOr<List<User>>>.Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        if(!await repository.CheckIfAdminUserAsync(request.AuthorizationDetails.UserId)) 
        {
            return Error.Unauthorized("Unauthorized");
        }
        return await repository.GetAllUsersAsync();
    }
}
