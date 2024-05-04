using UserManagement.Backend.Domain.Database;

namespace UserManagement.Backend.Application.Core.UserManagement.Queries.GetGroupsAndPermissions
{
    public class GetGroupsAndPermissionsQueryResponseDTO
    {
        public required List<Group> Groups { get; set; }
        public required List<Permission> Permissions { get; set; }
    }
}
