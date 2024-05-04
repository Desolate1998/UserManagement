namespace UserManagement.Backend.Domain.Database;

public class GroupPermission
{
    public required string GroupCode { get; set; }
    public required string PermissionCode { get; set; }
    public virtual Group? Group { get; set; }
    public virtual Permission?  Permission { get; set; }

    public static GroupPermission Create(string groupCode,string permissionCode)
    {
        return new() { GroupCode = groupCode, PermissionCode = permissionCode };
    }
}
