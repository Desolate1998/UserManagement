using ErrorOr;
using MediatR;
using UserManagement.Backend.Domain.Repositories_Interfaces;

namespace UserManagement.Backend.Application.Core.UserManagement.Queries.GetGroupsAndPermissions
{
    public class GetGroupsAndPermissionsQueryHandler(IUserManagementRepository repository) : IRequestHandler<GetGroupsAndPermissionsQuery, ErrorOr<GetGroupsAndPermissionsQueryResponseDTO>>
{
        async Task<ErrorOr<GetGroupsAndPermissionsQueryResponseDTO>> IRequestHandler<GetGroupsAndPermissionsQuery, ErrorOr<GetGroupsAndPermissionsQueryResponseDTO>>.Handle(GetGroupsAndPermissionsQuery request, CancellationToken cancellationToken)
        {
            if (!await repository.CheckIfAdminUserAsync(request.AuthorizationDetails.UserId))
            {
                return Error.Unauthorized("Unauthorized");
            }

            var groups = await repository.GetGroupsAsync();
            var permissions = await repository.GetPermissionAsync();

            return new GetGroupsAndPermissionsQueryResponseDTO()
            {
                Groups = groups,
                Permissions = permissions
            };
        }
    }
}
