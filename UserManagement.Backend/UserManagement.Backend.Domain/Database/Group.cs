namespace UserManagement.Backend.Domain.Database;

public class Group
{
    public required string Code { get; set; } 
    public required string Name { get; set; }
    public required string Description { get; set; }
    public virtual List<GroupPermission>? GroupPermissions { get; set; }
    public virtual List<UserGroup>? UserGroups { get; set; }
    public static Group Create(string code,string name, string description)
    {
        return new()
        {
            Description = description,
            Code = code,
            Name = name
        };
    }
}

