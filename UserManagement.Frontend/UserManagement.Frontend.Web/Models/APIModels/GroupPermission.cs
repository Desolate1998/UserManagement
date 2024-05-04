namespace UserManagement.Frontend.Web.Models.APIModels;

public class GroupPermission
{
    public required string GroupCode { get; set; }
    public required string PermissionCode { get; set; }
    public virtual Group? Group { get; set; }
    public virtual Permission? Permission { get; set; }
}
