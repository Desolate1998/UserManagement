namespace UserManagement.Backend.Domain.Database;

public class Permission
{
    public required string Code { get; set; }
    public required string Description { get; set; }
    public virtual List<GroupPermission>?  GroupPermissions { get; set; }

    public static Permission Create(string Code,string description)
    {
        return new()
        {
            Description = description,
            Code = Code,
        };
    }
}

