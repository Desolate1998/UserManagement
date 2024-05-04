namespace UserManagement.Frontend.Web.Models.APIModels;

public class GetGroupsAndPermissions
{
    public required List<Group> Groups { get; set; }
    public required List<Permission> Permissions { get; set; }
}
