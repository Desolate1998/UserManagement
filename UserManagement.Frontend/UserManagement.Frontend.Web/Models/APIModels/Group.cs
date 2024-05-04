using System.Text.Json;

namespace UserManagement.Frontend.Web.Models.APIModels;

public class Group
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public virtual List<GroupPermission>? GroupPermissions { get; set; }
    public virtual List<UserGroup>? UserGroups { get; set; }

    public string GetPermissions()
    {
        return JsonSerializer.Serialize(this.GroupPermissions!.Select(x=>x.PermissionCode).ToList());
    }
}

